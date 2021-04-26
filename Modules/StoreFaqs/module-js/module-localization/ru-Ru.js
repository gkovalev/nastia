var langStoreFaqs = [];

langStoreFaqs["StoreFaqsAwfull"] = "Ужасно";
langStoreFaqs["StoreFaqsBad"] = "Плохо";
langStoreFaqs["StoreFaqsNormal"] = "Нормально";
langStoreFaqs["StoreFaqsGood"] = "Хорошо";
langStoreFaqs["StoreFaqsExcelent"] = "Отлично";
langStoreFaqs["StoreFaqsAlreadyVote"] = "Вы уже голосовали";

langStoreFaqs["StoreFaqsValidEmail"] = "Некорректно введен email";
langStoreFaqs["StoreFaqsValidRequired"] = "Это поле обязательное для заполнения";

langStoreFaqs["StoreFaqsFormReplyTitle"] = "Ответить";
langStoreFaqs["StoreFaqsFormReplyName"] = "Имя";
langStoreFaqs["StoreFaqsFormReplyEmail"] = "Email";
langStoreFaqs["StoreFaqsFormReplyFaq"] = "Сообщение"; 
langStoreFaqs["StoreFaqsFormReplySend"] = "Отправить";
langStoreFaqs["StoreFaqsFormReplyDelete"] = "Удалить";
langStoreFaqs["StoreFaqsFormReplyCancel"] = "Отмена";

langStoreFaqs["StoreFaqsFormReplyErrorAdd"] = "Ошибка при добавлении отзыва";

function localizeStoreFaqs(param) {
    var p = param.toString();
    return langStoreFaqs[p] || '<span style="color:red;">NOT RESOURCED</span>';
};