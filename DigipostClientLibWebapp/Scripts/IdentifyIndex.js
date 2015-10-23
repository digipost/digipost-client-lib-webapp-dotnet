
$(function () {
    $("#tabs").tabs();
});

$(function () {
    $("form").submit(function () {
        if ($(this).valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    $("#IdentifyResultId").html(result);
                }
            });
        }
        return false;
    });
});