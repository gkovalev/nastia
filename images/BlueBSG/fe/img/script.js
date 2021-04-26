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
    path_images: '../images/GreenBSG/images/',


    divisions: [],
    choicewizards: [],
    available_categories: {},

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
            this.admin    = this.getXML('admin', true);

            // build html
            this.log('Start building process.', 'info');
            this.buildSlider();
            this.buildChoiceWizard();
            this.buildHeader();
            this.buildStage();
            this.buildTeaser();
            this.buildService();
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
        var $promostage = $('#dcp__promostage'),
            $container = $promostage.children('div'),
            img        = $promostage.children('img')[0],
            active_img,
            img_name;

        // two fading images
        if (this.choicewizards.length > 1) {
            active_img           = $container.children('img')[0];
            active_img.className = 'active';
            active_img.src       = this.path_images + prepareOutput(this.dealer.language) + '/stage_half_' + this.choicewizards[0].division + '.jpg';

            $container.children('img')[1].src = this.path_images + prepareOutput(this.dealer.language) + '/stage_half_' + this.choicewizards[1].division + '.jpg';

            // remove unused draft
            $promostage[0].removeChild(img);

        // single image
        } else {
            if (this.choicewizards.length) {
                // half length
                img_name = 'stage_half_' + this.choicewizards[0].division + '.jpg';
            } else if (this.divisions.length > 1) {
                // full length
                img_name = 'stage_full_combined.jpg';
            } else {
                // full length
                img_name = 'stage_full_' + this.divisions[0] + '.jpg';
            }

            img.src = this.path_images + prepareOutput(this.dealer.language) + '/' + img_name;

            // remove unused draft
            $promostage[0].removeChild($container[0]);
        }
    },

    buildChoiceWizard: function() {
        ChoiceWizard.init(this);
    },

    buildSlider: function() {
        var subcat_column_count = 2,
            sections            = [ 'products', 'accessories' ],
            available_cats      = [],
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

                    // add category to available categories for later use in choicewizard
                    available_cats.push(cat_data.id);

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

                            // add category to available categories for later use in choicewizard
                            available_cats.push(subcat_data.id);

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

                    this.available_categories[division] = available_cats;
                    available_cats = [];
                }
            }

        }

        if (!this.divisions.length) {
            throw new Exception('Could not fill any division slider with categories. Either the XML-data or the HTML-template is malformed.', 155);
        }

    },

    buildTeaser: function() {
        var teaser        = this.getXML(prepareOutput(this.dealer.language, false) + '/teaser'),
            $teaser_draft = $('div.dcp__teaser').clone().end().remove(),
            $head, $expand,
            division;

        for (d = 0; d < this.divisions.length; d++) {
            division = this.divisions[d];

            $teaser = $teaser_draft.clone();
            $head   = $teaser.children('div.head'),
            $expand = $teaser.children('div.expandable');

            $head.children('img')[0].src      = this.path_images + 'teaser/' + division + '_1.png';
            $head.children('h2')[0].innerHTML = prepareOutput(teaser[division].headline);
            $head.children('p')[0].innerHTML  = prepareOutput(teaser[division].subheadline);
            $head.children('a')[0].innerHTML  = prepareOutput(this.lang.teaser.more);

            $expand.children('img')[0].src     = this.path_images + 'teaser/' + division + '_2.png';
            $expand.children('p')[0].innerHTML = prepareOutput(teaser[division].text).nl2br();
            $expand.children('a')[0].innerHTML = prepareOutput(this.lang.teaser.less);

            (document.getElementById('division_' + division)).appendChild($teaser[0]);
        }
    },

    buildService: function() {
        var $service = $('#dcp__service'),
            service  = this.getXML(prepareOutput(this.dealer.language, false) + '_'
                                   + prepareOutput(this.dealer.country, false).toUpperCase() + '/service', true);

        if (!('enabled' in service) || (service.enabled !== 'true' && service.enabled !== '1')) {
            // remove dummy
            $service.remove();

            return;
        }

        var $tab_elements      = $service.children('ul'),
            $tab_container     = $tab_elements.eq(0),
            $content_container = $tab_elements.eq(1),

            tab_draft       = $tab_container.children('li').first().clone().end().remove()[0],
            content_draft   = $content_container.children('li').first().clone().end().remove()[0],

            i, tab_data, tab_name, temp,
            tab, content;

        if (!('tab' in service) || !service.tab.length) {
            // remove dummy
            $service.remove();

            return;
        }

        // set section headline
        $service.children('h2')[0].innerHTML = prepareOutput(this.lang.service.section_headline);

        // process the tabs
        for (i = 0; i < service.tab.length; i++) {
            tab_data = service.tab[i];

            tab_name = prepareOutput(tab_data.name, false);

            if (tab_name === '') {
                continue;
            }

            // tab navigation
            tab = tab_draft.cloneNode(true);
            $(tab).children('span').first().children('a')[0].innerHTML = tab_name;

            $tab_container[0].appendChild(tab);

            // tab content
            content = content_draft.cloneNode(true);
            $(content).children('img')[0].src = this.path_images + '/service/' + (i + 1) + '.png';

            if ('text1' in tab_data) {
                content.innerHTML += '<h5>' + prepareOutput(tab_data.text1, false) + '</h5>';
            }
            if ('text2' in tab_data) {
                content.innerHTML += '<h4>' + prepareOutput(tab_data.text2, false) + '</h4>';
            }
            if (('text3' in tab_data) || ('text4' in tab_data)) {
                temp = prepareOutput(tab_data.text3, false);
                if ('text4' in tab_data) {
                    temp += '<span>' + prepareOutput(tab_data.text4, false) + '</span>';
                }
                content.innerHTML += '<h4 class="phone">' + temp + '</h4>';
            }
            if ('text5' in tab_data) {
                content.innerHTML += '<span class="opening">' + prepareOutput(tab_data.text5, false).nl2br() + '</span>';
            }
            if ('text6' in tab_data) {
                content.innerHTML += '<span class="sub">' + prepareOutput(tab_data.text6, false) + '</span>';
            }
            if ('text7' in tab_data) {
                content.innerHTML += '<h3>' + prepareOutput(tab_data.text7, false) + '</h3>';
            }
            if (('text8' in tab_data) || ('text9' in tab_data)) {
                if ('text8' in tab_data) {
                    temp = '<b>' + prepareOutput(tab_data.text8, false) + '</b> ';
                }
                temp += prepareOutput(tab_data.text9, false);
                content.innerHTML += '<p>' + temp + '</p>';
            }

            $content_container[0].appendChild(content);
        }
    },

    buildFooter: function() {
        $('#dcp__footer').children('span')[0].innerHTML = prepareOutput(this.lang.branding.copyright, false);
    },

    determineMaxContentHeight: function() {
        this.max_content_height = this.$content_wrapper.outerHeight(true)
                                + this.$content_wrapper.offset().top;

        // add height of highest service tab
        var $service_tabs  = $('#dcp__service').children('ul').eq(1).children('li'),
            max_tab_height = 0,
            tab_height;
        if ($service_tabs.length) {
            $service_tabs.each(function() {
                tab_height = $(this).outerHeight(true);
                if (tab_height > max_tab_height) {
                    max_tab_height = tab_height;
                }
            });
            this.max_content_height += max_tab_height;
        }

        this.log('Determined max_content_height: ' + this.max_content_height, 'debug');
    },

    initUiElements: function() {
        /**
         * @section promo_fade
         */
        var $promostage = $('#dcp__promostage').children('div').first();
        // make sure there are at least two images to fade
        if ($promostage.children('img').length > 1) {
            setInterval(function() {
                $promostage.children('img').first().fadeOut(2000, function(){
                    $promostage.append(this);
                    $(this).removeClass('active').show();
                    $promostage.children('img').first().addClass('active');
                });
            }, 7000);
        }

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

        // set first tab elements active in service section
        $('#dcp__service').children('ul').each(function() {
            $(this).children('li').first().addClass('active');
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

var ChoiceWizard = {
    Frontend: null,
    filter_elems: null,
    division: null,

    $cw: null,
    $cw_rc: null,
    $cw_back: null,
    $form: null,
    $field_container: null,
    $field_submit: null,

    in_action: false,
    resultlist_initialized: false,

    $result_item_draft: null,

    init: function(frontend) {
        var cw, d, division, data;

        this.Frontend = frontend,
        this.$cw      = $('#dcp__results'),
        this.$form    = $('#dcp__promostage').children('form').first();

        // precheck for required language strings to avoid late exceptions
        this.precheckLangStrings();

        for (d = 0; d < this.Frontend.divisions.length; d++) {
            division = this.Frontend.divisions[d];

            // look first if there is a choicewizard activated for this division in the admin.xml
            if (this.Frontend.admin[division].choicewizard !== 'true' && this.Frontend.admin[division].choicewizard !== '1') {
                continue;
            }

            // load cw xml if not already done
            if (typeof cw === 'undefined') {
                cw = this.Frontend.getXML('choicewizard', true);
            }

            // parse choicewizard data and convert it into a more suitable structure
            data = this.prepareData({ length: 0 }, cw[division].filter, division, 0);

            if (!jQuery.isEmptyObject(data)) {
                // there is a choicewizard with data available for this division
                this.Frontend.choicewizards.push({ division: division, filter: data });
            }
        }

        // no choicewizard, remove dummy html and leave cw building process
        if (!this.Frontend.choicewizards.length) {
            this.$form.remove();
            this.Frontend.$content_wrapper[0].removeChild(this.$cw[0]);
            return;
        }

        this.Frontend.log(this.Frontend.choicewizards, 'dir');

        // set variables for further processing
        this.filter_elems = [
            $('#dcp__filter_division'),
            $('#dcp__filter_1'),
            $('#dcp__filter_2'),
            $('#dcp__filter_3')
        ];
        this.$field_container         = this.$form.children('fieldset');
        this.$field_submit            = this.$field_container.children('a');
        this.$cw_rc                   = $('#dcp__resultcontent');
        this.$cw_back                 = $('#dcp__back');

        // set language specific text (headline, subheadline, button caption)
        this.$field_container.children('h5')[0].innerHTML           = this.Frontend.lang.choicewizard.title;
        this.$field_container.children('p')[0].innerHTML            = prepareOutput(this.Frontend.lang.choicewizard.text, false);
        this.$field_submit.children('b').children('b')[0].innerHTML = this.Frontend.lang.choicewizard.start;

        this.buildFilterSelection();

        this.initAnimation();
    },

    precheckLangStrings: function() {
        for (var f in this.Frontend.lang.choicewizard.filters.filter) {
            if (typeof this.Frontend.lang.choicewizard.filters.filter[f] !== 'boolean') { // skip isPseudoArray flag
                this.Frontend.lang.choicewizard.filters.filter[f] = prepareOutput(this.Frontend.lang.choicewizard.filters.filter[f]);
            }
        }

        this.Frontend.lang.choicewizard.title = prepareOutput(this.Frontend.lang.choicewizard.title);
        this.Frontend.lang.choicewizard.choices.choice[0] = prepareOutput(this.Frontend.lang.choicewizard.choices.choice[0]);
        this.Frontend.lang.choicewizard.choices.choice[1] = prepareOutput(this.Frontend.lang.choicewizard.choices.choice[1]);
        this.Frontend.lang.choicewizard.choices.choice[2] = prepareOutput(this.Frontend.lang.choicewizard.choices.choice[2]);
        this.Frontend.lang.choicewizard.choices.choice[3] = prepareOutput(this.Frontend.lang.choicewizard.choices.choice[3]);
        this.Frontend.lang.choicewizard.choices.no_choice = prepareOutput(this.Frontend.lang.choicewizard.choices.no_choice);
        this.Frontend.lang.choicewizard.start = prepareOutput(this.Frontend.lang.choicewizard.start);
        this.Frontend.lang.choicewizard.section_headline = prepareOutput(this.Frontend.lang.choicewizard.section_headline);
        this.Frontend.lang.choicewizard.overview = prepareOutput(this.Frontend.lang.choicewizard.overview);
        this.Frontend.lang.choicewizard.matches = prepareOutput(this.Frontend.lang.choicewizard.matches);
        this.Frontend.lang.choicewizard.link = prepareOutput(this.Frontend.lang.choicewizard.link);
    },

    buildFilterSelection: function() {
        var that = this,
            $filter_division_options = this.filter_elems[0].children('option'),
            d, option, level;

        /**
         * Filter division
         */
        // cw for more than one division
        if (this.Frontend.choicewizards.length > 1) {
            this.removeUnusedFilterElems(
                Math.max(this.Frontend.choicewizards[0].filter.length, this.Frontend.choicewizards[1].filter.length),
                this.$field_container[0]
            );

            // set division filter
            $filter_division_options[0].innerHTML = this.Frontend.lang.choicewizard.choices.choice[0];

            for (d = 0; d < this.Frontend.choicewizards.length; d++) {
                option = document.createElement('option');
                option.value = d;
                option.innerHTML = prepareOutput(this.Frontend.lang[this.Frontend.choicewizards[d].division].name)
                this.filter_elems[0][0].appendChild(option);
            }

            this.filter_elems[0].data('level', 0);

            this.filter_elems[0].change(function() {
                that.division = this.value;
            });

            for (level = 1; level < this.filter_elems.length; level++) {
                this.initFilterElem(level, true);
            }

        // there is only one cw, remove division filter
        } else {
            this.division = 0;

            this.removeUnusedFilterElems(this.Frontend.choicewizards[0].filter.length, this.$field_container[0]);

            this.$field_container[0].removeChild(this.filter_elems[0][0]);

            for (level = 1; level < this.filter_elems.length; level++) {
                this.initFilterElem(level, level > 1);
            }
        }

        for (level = 0; level < this.filter_elems.length; level++) {
            this.filter_elems[level].change(function() {
                that.fillFilterElems(this);
            });
        }
    },

    initAnimation: function() {
        var that                     = this,
            $content                 = $('#dcp__content'),
            $cw_rl                   = $('#dcp__resultlist'),
            $spinner                 = $cw_rl.children('div').eq(1),
            was_hidden, content_height, cw_outer_height_addition;

        // calculate
        cw_outer_height_addition = $cw_rl.offset().top - $cw_rl.offsetParent().offset().top + 2;

        this.$cw.hide();

        // bind submit button click: fire submit event
        this.$field_submit.click(function() {
            that.$form.submit();
            return false;
        });

        // show cw results
        this.$form.submit(function(x,y,z) {
            // do nothing if the cw is already in action
            if (that.in_action) {
                return false;
            }
            that.in_action = true;

            var cats = that.getResultCategories();

            if (cats === false) {
                that.in_action = false;
                return false;
            }

            that.initResultlist();

            // animate
            content_height = $content.outerHeight(true) - parseInt($content.find('.dcp__section').eq(0).css('margin-top'));

            was_hidden = false;
            if (that.$cw.is(':hidden')) {
                $cw_rl.height(content_height - cw_outer_height_addition);
                was_hidden = true;
            } else {
                $cw_rl[0].className = 'loading';
                $spinner.show();
            }

            $content.hide();

            that.$cw.fadeIn(function() {
                // get results
                that.listResults(cats);

                // finally show results
                cw_rc_height = that.$cw_rc.outerHeight(true);
                that.$cw_rc[0].className = '';
                $cw_rl.animate({
                    height: cw_rc_height
                }, function () {
                    /**
                     * ugly workaround, actually used to fix randomly appearing height differences in ie6/7;
                     * but there is also a wrong height problem in safari 4 @ windows xp;
                     * whatever, its not a bad idea to enable it for all browsers,
                     * while the only disadvantage in modern browsers is some little more processing time
                     */
                    if (was_hidden) {
                        $cw_rl.animate({
                            height: that.$cw_rc.outerHeight(true)
                        })
                    }

                    $spinner.fadeOut(function() {
                        $cw_rl[0].className = '';

                        // content hight could be longer now, resize the iframe if necessary
                        that.Frontend.resizeIframe();

                        // action finished
                        that.in_action = false;
                    });
                });
            });

            return false;
        });

        // hide cw results
        this.$cw_back.click(function() {
            // do nothing if the cw is already in action
            if (that.in_action) {
                return false;
            }
            that.in_action = true;

            $cw_rl[0].className = 'loading';
            $spinner.show();
            that.$cw_rc[0].className = 'hidden';

            $cw_rl.animate({
                height: content_height - cw_outer_height_addition
            }, function() {
                that.$cw.fadeOut(function() {
                    $content.fadeIn(function() {
                        that.in_action = false;
                    });
                });
            });

            return false;
        });
    },

    initResultlist: function() {
        if (this.resultlist_initialized) {
            return false;
        }

        // set texts
        this.$cw.children('h2')[0].innerHTML    = this.Frontend.lang.choicewizard.section_headline;
        this.$cw_back[0].innerHTML              = this.Frontend.lang.choicewizard.overview;
        this.$cw_rc.children('h3')[0].innerHTML = this.Frontend.lang.choicewizard.matches;

        // create and prepare result item draft
        this.$result_item_draft = this.$cw_rc.children('ul').children('li').eq(0).clone().end().remove();
        this.$result_item_draft.children('a')[0].innerHTML = this.Frontend.lang.choicewizard.link;

        // set initialized flag
        this.resultlist_initialized = true;
    },

    listResults: function(cats) {
        var item_container = this.$cw_rc.children('ul')[0],
            c, url, headline, $item;

        // empty container
        item_container.innerHTML = '';

        for (c = 0; c < cats.length; c++) {
            // check if it really got an url
            url = prepareOutput(this.Frontend.dealer.categories.category[cats[c]].url, false);
            if (url === '') {
                this.Frontend.log('Result item with the id ' + cats[c] + ' has no url set, skipping.', 201);
                continue;
            }

            // create a copy of the draft
            $item = this.$result_item_draft.clone();

            // dirty workaround for removing forced linebreaks used in slider
            headline = prepareOutput(this.Frontend.lang.categories.category[cats[c]].name).replace('- ', '');

            // set content
            $item.children('img')[0].src = this.Frontend.path_images + 'categories/' + cats[c] + '_wide.png';
            $item.children('h4')[0].innerHTML = headline;
            $item.children('p')[0].innerHTML = prepareOutput(this.Frontend.lang.categories.category[cats[c]].text, false);
            $item.children('a')[0].href = url;

            item_container.appendChild($item[0]);
        }
    },

    prepareData: function(filter_obj, cw, division, level, parent_filter) {
        var data, i, j;

        for (i = 0; i < cw.length; i++) {
            // empty data array
            data = [];

            // it's a filter tree
            if ('filter' in cw[i]) {
                // loop through the leafs
                for (j = 0; j < cw[i].filter.length; j++) {
                    data.push(cw[i].filter[j].id);
                }

                if (typeof filter_obj[level] === 'undefined') {
                    // add a new level
                    filter_obj[level] = {};
                    filter_obj.length++;
                }

                // add new filter with child filters on current level
                filter_obj[level][cw[i].id] = { type: 'filter', parent: parent_filter, data: data };

                // recursively follow the tree and look for more filter/category trees
                filter_obj = this.prepareData(filter_obj, cw[i].filter, division, level + 1, cw[i].id);

            // it's a category tree
            } else if ('category' in cw[i]) {
                // loop through the leafs
                for (j = 0; j < cw[i].category.length; j++) {
                    if (this.Frontend.available_categories[division].indexOf(cw[i].category[j].id) >= 0) {
                        // category leaf is an available category, collect it!
                        data.push(cw[i].category[j].id);
                    }
                }

                // no categories collected
                if (!data.length) {
                    // reverse remove filter in tree
                    filter_obj = this.reverseRemoveFilter(filter_obj, level, cw[i].id, parent_filter);

                    // loop on
                    continue;
                }

                if (typeof filter_obj[level] === 'undefined') {
                    // add a new level
                    filter_obj[level] = {};
                    filter_obj.length++;
                }

                // add new filter with category childs on current level
                filter_obj[level][cw[i].id] = { type: 'category', parent: parent_filter, data: data };

            } else {
                // can happen if choicewizard.xml is malformed, for example last filter element in line has no category elements
                filter_obj = this.reverseRemoveFilter(filter_obj, level, cw[i].id, parent_filter);
            }
        }

        return filter_obj;
    },

    reverseRemoveFilter: function(filter_obj, level, filter_id, parent_filter) {
        // go up one level
        level--;

        // leave if we are at root or even higher
        if (typeof parent_filter === 'undefined' || level < 0 || !(parent_filter in filter_obj[level])) {
            return filter_obj;
        }

        // get position of filter in filter array of the parent and drop it there
        var pos = filter_obj[level][parent_filter].data.indexOf(filter_id);
        filter_obj[level][parent_filter].data.splice(pos, 1);

        if (!filter_obj[level][parent_filter].data.length) {
            // the parent has no other filters after we dropped the last one, we can drop the parent also
            this.reverseRemoveFilter(filter_obj, level, parent_filter, filter_obj[level][parent_filter].parent);
            delete filter_obj[level][parent_filter];

            // if we remove the the last filter on the current filter level we can drop the whole filter level
            if (jQuery.isEmptyObject(filter_obj[level])) {
                delete filter_obj[level];
                filter_obj.length--;
            }
        }

        return filter_obj;
    },

    removeUnusedFilterElems: function(max_filter_level, parent_container) {
        // remove filter elements which will never be used because of lack of data
        while (this.filter_elems.length - 1 > max_filter_level) {
            parent_container.removeChild(this.filter_elems[this.filter_elems.length - 1][0]);
            this.filter_elems.pop();
        }
    },

    initFilterElem: function(level, inactive) {
        var $elem         = this.filter_elems[level],
            first_option = $elem[0].options[0],
            option_draft, option, item;

        $elem.data('level', level);

        $elem[0].disabled = inactive;

        if (inactive) {
            first_option.innerHTML = this.Frontend.lang.choicewizard.choices.choice[level];
            return;
        }

        first_option.innerHTML = this.Frontend.lang.choicewizard.choices.choice[level];
        $elem[0].appendChild(first_option);

        // reduce options to description option
        $elem[0].options.length = 1;

        option_draft = document.createElement('option');

        for (item in this.Frontend.choicewizards[this.division].filter[level - 1]) {
            option           = option_draft.cloneNode(true);
            option.value     = item;
            option.innerHTML = this.Frontend.lang.choicewizard.filters.filter[item];
            $elem[0].appendChild(option);
        }
    },

    setDisabledFilter: function(level, no_choice) {
        var elem = this.filter_elems[level][0];
        var text = no_choice === true
                 ? this.Frontend.lang.choicewizard.choices.no_choice
                 : this.Frontend.lang.choicewizard.choices.choice[level];

        elem.options.length = 1;
        elem.options[0].val = '';
        elem.options[0].innerHTML = prepareOutput(text);
        elem.disabled = true;
    },

    fillFilterElems: function(cur_filter_elem) {
        var cur_level = $(cur_filter_elem).data('level');
        var selected_filter_id = this.filter_elems[cur_level][0].value.trim();

        var level, elem;

        if (selected_filter_id === '') {
            for (level = cur_level + 1; level < this.filter_elems.length; level++) {
                this.setDisabledFilter(level);
            }

            this.setSubmitButton(false);

            return;
        }

        if ((cur_level + 1) >= (this.filter_elems.length)) {
            // we are on the max level

            this.$form.submit(); // auto submit
            this.setSubmitButton(true);

            return;
        }

        if (cur_level === 0) {
            option_draft = document.createElement('option');
            elem = this.filter_elems[cur_level + 1][0];
            elem.options.length = 1;
            elem.options[0].val = '';
            elem.options[0].innerHTML = this.Frontend.lang.choicewizard.choices.choice[cur_level + 1];

            var filters = this.Frontend.choicewizards[this.division].filter[cur_level];
            var filter;
            var filters_length = 0;
            for (filter in filters) {
                /* 2 EXTRACT START */
                option           = option_draft.cloneNode(true);
                option.value     = filter;
                option.innerHTML = this.Frontend.lang.choicewizard.filters.filter[filter];
                elem.appendChild(option);
                filters_length++;
                /* 2 EXTRACT STOP */
            }

            elem.disabled = false;

            if (filters_length === 1) {
                elem.selectedIndex = 1;
                elem.options[1].setAttribute('selected', true); // workaround for ie6
                this.fillFilterElems(elem);
                return;
            }

            for (level = cur_level + 2; level < this.filter_elems.length; level++) {
                this.setDisabledFilter(level);
            }

            this.setSubmitButton(false);
            return;
        } else {
            var item = this.Frontend.choicewizards[this.division].filter[cur_level - 1][selected_filter_id];
        }

        if (item.type === 'category') {
            for (level = cur_level + 1; level < this.filter_elems.length; level++) {
                /* 1 EXTRACT START */
                this.setDisabledFilter(level, true);
                /* 1 EXTRACT STOP */
            }

            this.$form.submit(); // auto submit
            this.setSubmitButton(true);

        } else if (item.type === 'filter') {
            option_draft = document.createElement('option');
            elem = this.filter_elems[cur_level + 1][0];
            elem.options.length = 1;
            elem.options[0].val = '';
            elem.options[0].innerHTML = this.Frontend.lang.choicewizard.choices.choice[cur_level + 1];

            var filters = this.Frontend.choicewizards[this.division].filter[cur_level - 1][selected_filter_id].data;
            var filter;
            for (f = 0; f < filters.length; f++) {
                filter = filters[f];
                /* 2 EXTRACT START */
                option           = option_draft.cloneNode(true);
                option.value     = filter;
                option.innerHTML = this.Frontend.lang.choicewizard.filters.filter[filter];
                elem.appendChild(option);
                /* 2 EXTRACT STOP */
            }

            elem.disabled = false;

            if (filters.length === 1) {
                elem.selectedIndex = 1;
                elem.options[1].setAttribute('selected', true); // workaround for ie6
                this.fillFilterElems(elem);
                return;
            }

            for (level = cur_level + 2; level < this.filter_elems.length; level++) {
                this.setDisabledFilter(level);
            }

            this.setSubmitButton(false);
        }
    },

    getResultCategories: function() {
        var level = this.filter_elems.length - 1,
            $filter_elem, filter_id, selected;

        // get last not disabled filter
        do {
            $filter_elem = this.filter_elems[level];
            level--;
        } while (level && $filter_elem[0].disabled);

        filter_id = $filter_elem[0].value.trim();

        // invalid selection, manipulation?
        if (filter_id === '') {
            return false;
        }

        selected = this.Frontend.choicewizards[this.division].filter[level][filter_id];

        // not a category but a filter, manipulation?
        if (selected.type !== 'category') {
            return false;
        }

        return selected.data;
    },

    setSubmitButton: function(active) {
        this.$field_submit[0].className = active !== true ? 'inactive' : '';
    }

};

$(document).ready(function() { Frontend.init(); });
