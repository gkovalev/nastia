(function ($) {


    var advantshop = Advantshop
    , scriptManager = advantshop.ScriptsManager;

    var vote = function (selector) {
        this.$obj = advantshop.GetJQueryObject(selector);

        return this;
    };

    advantshop.NamespaceRequire('Advantshop.ScriptsManager');
    scriptManager.Vote = vote;

    vote.prototype.enumState = { Default: 'default', Result: 'result' };

    var state = vote.prototype.enumState.Default;

    vote.prototype.InitTotal = function () {
        var objects = $('[data-plugin ="vote"]');

        for (var i = 0, arrLength = objects.length; i < arrLength; i += 1) {
            vote.prototype.Init(objects.eq(i));
        }
    };

    $(vote.prototype.InitTotal); // call document.ready

    vote.prototype.Init = function (selector) {
        var voteObj = new vote(selector);

        voteObj.GenerateHtml();
        voteObj.BindEvent();

        return voteObj;
    };

    vote.prototype.GenerateHtml = function () {
        var voteObj = this;

        if (state === null) {
            throw new Error(localize("votingErrorGetResults"));
        }

        var progressMini = new scriptManager.Progress.prototype.Init(voteObj.$obj);
        progressMini.Show();

        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: 'httphandlers/voting/getvotingdata.ashx',
            success: function (data) {
                if (data == null) {
                    throw new Error(localize("votingErrorGetResults"));
                }

                var url = !data.isVoted ? 'js/plugins/vote/templates/default.html' : 'js/plugins/vote/templates/result.html';

                var html = new EJS({ url: url }).render(data);

                voteObj.$obj.html(html);
            },
            error: function (data) {
                throw new Error(localize("votingErrorGetResults"));
            },
            complete: function () {
                progressMini.Hide();
            }
        });
    };

    vote.prototype.BindEvent = function () {
        var voteObj = this;

        voteObj.$obj.on('click.vote', '[data-vote-control="submit"]', function () {

            var selectedAnswer = voteObj.$obj.find('input[name="vote"]:checked');

            if (selectedAnswer.length === 0) {
                return;
            }

            voteObj.addVote(selectedAnswer.val());
        });
    };


    vote.prototype.addVote = function (answerId) {
        var voteObj = this;

        var progressMini = new scriptManager.Progress.prototype.Init(voteObj.$obj);
        progressMini.Show();

        $.ajax({
            dataType: 'json',
            type: 'POST',
            url: 'httphandlers/voting/addvote.ashx',
            data: { answerId: answerId },
            success: function (data) {

                if (data === false) {
                    throw new Error(localize("votingErrorAddVoice"));
                }

                state = vote.prototype.enumState.Result;

                voteObj.GenerateHtml();

            },
            error: function () {
                throw new Error(localize("votingErrorAddVoice"));
            },
            complete: function () {
                progressMini.Hide();
            }
        });
    };

})(jQuery);