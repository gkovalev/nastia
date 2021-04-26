String.prototype.trim = function() {
    var str = this.replace(/^\s+/, ''),
        end = str.length - 1,
        ws  = /\s/;

    while (ws.test(str.charAt(end))) {
        end--;
    }

    return str.slice(0, end + 1);
};

// http://phpjs.org/functions/nl2br:480
String.prototype.nl2br = function() {
    return (this + '').replace(/([^>\r\n]?)(\r\n|\n\r|\r|\n)/g, '$1' + '<br />' + '$2');
};

Array.prototype.indexOf = function(obj) {
    var i = this.length;
    while (i--) {
        if (this[i] === obj) {
            return i;
        }
    }
    return -1;
};

function Exception(message, code) {
    this.message = message;
    this.code    = code;
}
Exception.prototype.toString = function() {
    return this.code + ': ' + this.message;
};

function xmlToObject(xml, useIdAsIndex) {
    var obj = {},
        nodeNameExceptions = [ 'category', 'filter', 'tab', 'choice' ],
        node, cur, id, val, a, i, temp;

    for (i = 0; i < xml.childNodes.length; i++) {
        node = xml.childNodes[i];

        // object value
        if (node.nodeType === 1) {
            // recursively process nodes
            cur = xmlToObject(node, useIdAsIndex);

            // add attributes
            if (node.attributes) {
                for (a = 0; a < node.attributes.length; a++) {
                    cur[node.attributes[a].nodeName] = node.attributes[a].nodeValue;
                }
            }

            // add child elements
            if ((node.nodeName in obj) || nodeNameExceptions.indexOf(node.nodeName) >= 0) {
                if (typeof obj[node.nodeName] === 'undefined') {
                    if (useIdAsIndex) {
                        obj[node.nodeName] = { isPseudoArray: true }; // create object
                    } else {
                        obj[node.nodeName] = []; // create array
                    }
                } else if ((useIdAsIndex && !obj[node.nodeName].isPseudoArray) || (!useIdAsIndex && !obj[node.nodeName].length)) {
                    temp = obj[node.nodeName];
                    if (useIdAsIndex && ('id' in temp)) {
                        obj[node.nodeName] = { isPseudoArray: true }; // create object
                        obj[node.nodeName][temp.id] = temp;
                    } else {
                        obj[node.nodeName] = []; // create array
                        obj[node.nodeName].push(temp);
                    }
                }

                id = node.getAttribute('id');
                if (useIdAsIndex && id) {
                    obj[node.nodeName][id] = cur;
                } else {
                    obj[node.nodeName].push(cur);
                }
            } else {
                obj[node.nodeName] = cur;
            }

        // text value
        } else if (node.nodeType === 3 || node.nodeType === 4) {
            val = node.nodeValue.trim();
            if (val) {
                obj = val;
            }
        }
    }
    return obj;
}

function prepareOutput(str, required) {
    if (typeof required !== 'boolean') {
        required = true; // set default value, if parameter not set
    }
    if (typeof str !== 'string') {
        if (required) {
            throw new Exception('Output value is of type "' + typeof str + '", instead of type string.', 161);
        } else {
            str = '';
        }
    } else if (str === '') {
        if (required) {
            throw new Exception('Output string is empty.', 162);
        }
    }
    return str;
}

var Frontend = {
    debug: 0,
    console_type_map: { 'error': 1, 'debug': 2, 'dir': 2, 'info': 3, 'time': 3, 'timeEnd': 3 },

    iframe_id: 'bosch-bsg-frontend',

    path_xml: '../xml/',
    path_images: '../images/',

    divisions: [],

    init: function() {
        try {

            this.iframe = window.top.document.getElementById(this.iframe_id);

            if (top != self && (!this.iframe || this.iframe.nodeName.toUpperCase() !== 'IFRAME')) {
                throw new Exception('Could not find IFrame element with the id="' + this.iframe_id + '".', 110);
            }

            // set debug level from iframe attribute
            this.debug = (this.iframe && this.iframe.getAttribute('debug')) || this.debug;
            this.log('Debug Level: ' + this.debug, 'error');

            this.log('Complete processing time spend for JavaScript logic', 'time');

            this.$content_wrapper = $('#dcp__limiter');

            // parse xmls
            this.dealer   = this.getXML('dealer');
            this.lang     = this.getXML(prepareOutput(this.dealer.language, false) + '/lang');
            this.teaser   = this.getXML(prepareOutput(this.dealer.language, false) + '/teaser');
            this.admin    = this.getXML('admin', true);

            // build html
            this.log('Start building process.', 'info');
            this.buildHeader();
            this.buildStage();
            this.buildSlider();
            this.buildTeaser();
            this.buildFooter();
            this.log('Building process completed.', 'info');

            // get max possible content height
            this.determineMaxContentHeight();

            // initialize ui elements (bindings, slider, fades, etc)
            this.log('Initializing UI elements.', 'info');
            this.initUiElements();
            this.log('UI elements initialized.', 'info');

            // finally show it
            this.log('Finally resizing the iframe and showing the content.', 'info');
            this.show();
            this.log('Finished.', 'info');

            this.log('Complete processing time spend for JavaScript logic', 'timeEnd');

        } catch (e) {
            // additionally write to console
            this.log(e, 'error');

            if (!e || !e.code) {
                e = { code: 0, message: 'An error occured.' };
            }

            switch (e.code) {
                case 110:
                    // Could not get IFrame element
                    this.showError(e.message);
                    break;
                case 120:
                    // Failed to load XML file
                    this.showError(e.message);
                    break;
                default:
                    // try to read error message from lang.xml
                    if (this.lang && this.lang.misc && this.lang.misc.misconfigured) {
                        this.showError(prepareOutput(this.lang.misc.misconfigured, false));
                    // show default error message
                    } else {
                        this.showError(e.message);
                    }
                    break;
            }
        }
    },

    log: function(obj, type) {
        if (typeof console !== 'undefined' && console !== null
            && typeof console[type] === 'function' && this.console_type_map[type] <= this.debug
        ) {
            console[type](obj);
        }
    },

    showError: function(message) {
        $('div.spinner').remove();
        $('body').append($('<p>').addClass('system_error').html(message));
    },

    getXML: function(name, notUseIdAsIndex) {
        var url = this.path_xml + name + '.xml',
            response = $.ajax({
            url: url,
            dataType: 'xml',
            async: false,
            cache: false,
            error: function(xhrObj, textStatus, errorThrown) {
                throw new Exception('Failed to load XML file "' + url + '":\n'
                                    + textStatus + '\n' + errorThrown, 120);
            }
        }).responseXML;

        this.log('Starting to convert XML response of "' + url + '" to object...', 'info');

        response = xmlToObject(response, !notUseIdAsIndex);

        this.log(response, 'dir');
        this.log('Finished XML to object conversion of "' + url + '".', 'info');

        response = response.root;

        return response;
    },

    aggregatedCategoriesObj: function(container) {
        if (typeof container !== 'object') {
            return false;
        }

        if (!('category' in container)) {
            throw new Exception('There are no category entries in the given container.', 151);
        }

        var acats      = container.category,
            obj        = [],
            cats_count, mcats, lcats, cat, subcats, i;

        cats_count = acats.length;
        mcats      = this.dealer.categories.category;
        lcats      = this.lang.categories.category;

        if (!mcats) {
            throw new Exception('No category entries found in dealer.xml.', 157);
        }

        for (i = 0; i < cats_count; i++) {
            cat = {};

            if (!(acats[i].id in lcats)) {
                throw new Exception('Category entry for id=' + acats[i].id + ' is missing in lang.xml.', 152);
            }

            cat.name = prepareOutput(lcats[acats[i].id].name, false);
            if (cat.name === '') {
                throw new Exception('Category name entry for id=' + acats[i].id + ' is missing or empty in lang.xml.', 153);
            }

            if ('category' in acats[i]) {
                subcats = this.aggregatedCategoriesObj(acats[i]);
                if (subcats.length) {
                    cat.subcats = subcats;
                }
            }

            cat.url = '';
            if (acats[i].id in mcats) {
                cat.url = mcats[acats[i].id].url.trim();
            }
            if (!cat.url && !('subcats' in cat)) {
                continue;
            }

            cat.id = acats[i].id;

            obj.push(cat);
        }

        if (('min_count' in container) && obj.length && obj.length < container.min_count) {
            throw new Exception('Not enough categories, only ' + obj.length + ' given while ' + container.min_count + ' are required.', 154);
        }

        return obj;
    },

    buildHeader: function() {
        var img_logo = $('#dcp__branding').children('img')[0];
        img_logo.src = this.path_images + prepareOutput(this.dealer.language) + '/logo.png';
        img_logo.alt = prepareOutput(this.lang.branding.slogan);
        document.title = prepareOutput(this.lang.branding.slogan);
    },

    buildStage: function() {
        $('#dcp__promostage').children('img')[0].src = this.path_images + prepareOutput(this.dealer.language) + '/stage_full_diy.jpg';
    },

    buildSlider: function() {
        var subcat_column_count = 2,
            sections            = [ 'products', 'accessories' ],
            has_categories,

            $slider_draft      = $('div.slider').first().clone().end().remove(),

            $tab_elements      = $slider_draft.children('ul'),
            $tab_container     = $tab_elements.eq(0),
            $content_container = $tab_elements.eq(1),

            $cat_dummies    = $slider_draft.find('ul.overview').children('li'),
            subcat_draft    = $slider_draft.find('ul.menulist').children('li').first().clone().end().remove()[0],
            tab_draft       = $tab_container.children('li').first().clone().end().remove()[0],
            content_draft   = $content_container.children('li').first().clone().end().remove()[0],
            cat_draft       = $cat_dummies.not('.submenu').first().clone().end().remove()[0],
            cat_w_sub_draft = $cat_dummies.filter('.submenu').first().clone().end().remove()[0],

            division, section, cats_data, cat_data, subcat_data, i, j, k,
            division_container, $slider, tab, content, cat_container, subcat_container, cat, subcat, $childs, li, a;

        for (division in this.admin) { // FIXME: divisions should be wrapped in extra container
            $slider = $slider_draft.clone();

            $tab_container     = $slider.children('ul').eq(0);
            $content_container = $slider.children('ul').eq(1);

            if (!(division in this.lang)) {
                throw new Exception('Missing values for division "' + division + '" in lang.xml.', 156);
            }

            has_categories = false;

            for (i = 0; i < sections.length; i++) {
                section = sections[i];

                cats_data = this.aggregatedCategoriesObj(this.admin[division][section]);

                if (!cats_data.length) {
                    continue;
                }

                /**
                 * tab navigation
                 */
                // create a clone
                tab = tab_draft.cloneNode(true);

                // set attributes and content
                $(tab).find('a')[0].innerHTML = prepareOutput(this.lang[division][section]);

                // insert element
                $tab_container[0].appendChild(tab);

                /**
                 * tab content
                 */
                // create a clone
                content = content_draft.cloneNode(true);

                cat_container    = $(content).find('ul.overview')[0];
                subcat_container = $(content).find('ul.menulist')[0];

                // set categories
                for (j = 0; j < cats_data.length; j++) {
                    cat_data = cats_data[j];

                    // category with subcats
                    if ('subcats' in cat_data)  {
                        // create a clone
                        cat = cat_w_sub_draft.cloneNode(true);

                        /**
                         * set submenus
                         */
                        // create a clone
                        subcat = subcat_draft.cloneNode(true);

                        // get subcategory columns
                        $childs = $(subcat).children('ul');

                        // process single subcategories
                        for (k = 0; k < cat_data.subcats.length; k++) {
                            subcat_data = cat_data.subcats[k];

                            li = document.createElement('li');

                            a           = document.createElement('a');
                            a.href      = subcat_data.url;
                            a.innerHTML = subcat_data.name;

                            li.appendChild(a);

                            $childs[k % subcat_column_count].appendChild(li);
                        }

                        // remove empty subcategory columns for valid (x)html
                        $childs.filter(':empty').remove();

                        // insert subcategory item
                        subcat_container.appendChild(subcat);

                    // category without subcats
                    } else {
                        // create a clone
                        cat = cat_draft.cloneNode(true);

                        // set attributes, if it is not a subcat, it has a link
                        $(cat).children('a')[0].href = cat_data.url;
                    }

                    // set attributes which are in common
                    $(cat).find('img')[0].src        = this.path_images + 'categories/' + cat_data.id + '.gif';
                    $(cat).find('span')[0].innerHTML = cat_data.name;

                    // insert category item
                    cat_container.appendChild(cat);
                }

                // remove empty subcategory container for valid (x)html
                if (!$(subcat_container).children().length) {
                    $(subcat_container).remove();
                }

                // insert tab content
                $content_container[0].appendChild(content);

                // categories were added for sure for the current division
                has_categories = true;
            }

            if (has_categories) {
                division_container = document.getElementById('division_' + division);

                if (division_container !== null) {
                    // set headline
                    $slider.children('h2')[0].innerHTML = prepareOutput(this.lang[division].name);

                    // add content
                    division_container.appendChild($slider[0]);

                    // set first tab elements active
                    $tab_container.children('li').first().addClass('active');
                    $content_container.children('li').first().addClass('active');

                    // add division to available division array
                    this.divisions.push(division);
                }
            }

        }

        if (!this.divisions.length) {
            throw new Exception('Could not fill any division slider with categories. Either the XML-data or the HTML-template is malformed.', 155);
        }

    },

    buildTeaser: function() {
        var $teaser = $('div.dcp__teaser'),
            $head   = $teaser.children('div.head'),
            $expand = $teaser.children('div.expandable');

        $head.children('img')[0].src      = this.path_images + 'teaser/diy_1.png';
        $head.children('h2')[0].innerHTML = prepareOutput(this.teaser.diy.headline);
        $head.children('p')[0].innerHTML  = prepareOutput(this.teaser.diy.subheadline);
        $head.children('a')[0].innerHTML  = prepareOutput(this.lang.teaser.more);

        $expand.children('img')[0].src     = this.path_images + 'teaser/diy_2.png';
        $expand.children('p')[0].innerHTML = prepareOutput(this.teaser.diy.text).nl2br();
        $expand.children('a')[0].innerHTML = prepareOutput(this.lang.teaser.less);
    },

    buildFooter: function() {
        $('#dcp__footer').children('span')[0].innerHTML = prepareOutput(this.lang.branding.copyright, false);
    },

    determineMaxContentHeight: function() {
        this.max_content_height = this.$content_wrapper.outerHeight(true)
                                + this.$content_wrapper.offset().top;
        this.log('Determined max_content_height: ' + this.max_content_height, 'debug');
    },

    initUiElements: function() {
        /**
         * @section tabnav
         */
        $('ul.tabnavigator a').live('click', function(){
            var $li      = $(this).parents('li'),
                $content = $li.parent().next('ul.tabcontent');

            // Tabnavigator active-state
            $li.addClass('active').siblings().removeClass('active');

            // Tabcontent active-state
            $content.children('li').removeClass('active').eq($li.index()).addClass('active');

            return false;
        });


        /**
         * @section slider
         */
        var $slide_content = $('li.slide-content'),
            IS_ROTATING = false; // initiate rotating status of slider carousel


        // change rotating status when pushing the carousel
        $slide_content.find('a.buttons').click(function() {
            IS_ROTATING = true;
        });

        // needs iteration, because plugin does not properly handle multiple elements
        $slide_content.each(function() {
            $(this).tinycarousel({
                display: 3,
                callback: function() {
                    IS_ROTATING = false;
                }
            });
        });

        // submenu on hovering
        var $items    = $slide_content.find('ul.overview > li.submenu'),
            $submenus = $slide_content.find('ul.menulist > li'),
            $hidden_siblings = $(),
            $item,
            idx, outer_left, outer_right; // vars for shadow workaround
        $.merge($items, $submenus)
            .mouseover(function() {
                if (IS_ROTATING || $(this).hasClass('hover')) {
                    return;
                }

                // items
                if ($(this).hasClass('item')) {
                    $item = $(this);
                // submenu
                } else {
                    $item = $items.eq($submenus.index(this));
                }

                // show hover on item
                $submenus.eq($items.index($item)).addClass('hover');

                // show submenu
                $item.addClass('hover');

                /* shadow workaround - START */
                idx = $item.index();

                // determine whether and on which outer side we are
                outer_left  = (idx % 3 === 0);
                outer_right = ((idx +1) % 3 === 0);

                // we are on the outer side
                if (outer_left || outer_right) {
                    if (outer_left) {
                        $hidden_siblings = $.merge($item.prev(), $item.next().next().next());
                    } else {
                        $hidden_siblings = $.merge($item.prev().prev().prev(), $item.next());
                    }
                    // hide outer items
                    $hidden_siblings.css('visibility', 'hidden');

                    // css rule for this class adds margin + padding on the outer side of the viewport so that the shadow is not overlapped
                    $item.parents('div.viewport').addClass('hovering');
                }
                /* shadow workaround - END */
            })
            .mouseout(function() {
                if (IS_ROTATING) {
                    return;
                }

                // make hidden siblings visible
                $hidden_siblings.css('visibility', 'visible');

                // items
                if ($(this).hasClass('item')) {
                    $item = $(this);
                // submenu
                } else {
                    $item = $items.eq($submenus.index(this));
                }

                // hide hover on item
                $item.removeClass('hover');

                // hide submenu
                $submenus.eq($items.index($item)).removeClass('hover');

                /* shadow workaround - START */
                $item.parents('div.viewport').removeClass('hovering');
                /* shadow workaround - END */
            });


        /**
         * @section expandable
         */
        // expand
        var $teaser = $('div.dcp__teaser');
        $teaser.find('a.expand').click(function(){
            $(this).fadeOut().parent().next('div.expandable').slideDown().children('a').fadeIn();

            return false;
        });

        // collapse
        $teaser.find('a.collapse').click(function(){
            $(this).fadeOut().parent().slideUp().prev('div.head').find('a.expand').fadeIn();

            return false;
        });

        $teaser.children('div.expandable').hide();
    },

    resizeIframe: function(callback, height) {
        callback = callback || $.noop;
        height   = height || this.$content_wrapper.outerHeight(true) + this.$content_wrapper.offset().top;

        if (height <= $(this.iframe).height()) {
            return false;
        }

        $(this.iframe).animate({
            height: height
        }, 700, callback);
    },

    show: function() {
        var that = this,
            callback = function() {
            var $body    = $('body'),
                $spinner = $body.children('div.spinner');

            that.$content_wrapper.removeClass('hidden');
            $spinner.animate({
                opacity: '0.0'
            }, 700, function() {
                $body.removeClass('loading');
            });
        };

        if (this.iframe) {
            this.resizeIframe(callback, this.max_content_height);
        } else {
            callback();
        }
    }

};

$(document).ready(function() { Frontend.init(); });
