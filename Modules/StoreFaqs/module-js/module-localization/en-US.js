var langStoreFaqs = [];

langStoreFaqs['StoreFaqsAwfull'] = 'Awfull';
langStoreFaqs['StoreFaqsBad'] = 'Bad';
langStoreFaqs['StoreFaqsNormal'] = 'Normal';
langStoreFaqs['StoreFaqsGood'] = 'Good';
langStoreFaqs['StoreFaqsExcelent'] = 'Excelent';
langStoreFaqs['StoreFaqsAlreadyVote'] = 'You have already voted';

langStoreFaqs["StoreFaqsValidEmail"] = "Incorrect email";
langStoreFaqs["StoreFaqsValidRequired"] = "This field is required";

langStoreFaqs["StoreFaqsFormReplyTitle"] = "Reply";
langStoreFaqs["StoreFaqsFormReplyName"] = "Name";
langStoreFaqs["StoreFaqsFormReplyEmail"] = "Email";
langStoreFaqs["StoreFaqsFormReplyFaq"] = "Message";
langStoreFaqs["StoreFaqsFormReplySend"] = "Send";
langStoreFaqs["StoreFaqsFormReplyDelete"] = "Remove";
langStoreFaqs["StoreFaqsFormReplyCancel"] = "Cancel";

langStoreFaqs["StoreFaqsFormReplyErrorAdd"] = "Error adding Faqs";

function localizeStoreFaqs(param) {
    var p = param.toString();
    return langStoreFaqs[p] || '<span style="color:red;">NOT RESOURCED</span>';
};