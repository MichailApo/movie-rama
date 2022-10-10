$(function () {
    $('.submitLike').on('click', function (evt) {
        evt.preventDefault();
        let url = "movie/" + $(this).data("id") + "/like"
        $.ajax({
            type: "POST",
            url: url
        })
            .done(function () {
            })
            .fail(function (res) {
                handleFail(res);
            });
    });

    $('.submitHate').on('click', function (evt) {
        evt.preventDefault();
        let url = "movie/" + $(this).data("id") + "/hate"
        $.ajax({
            type: "POST",
            url: url
        })
            .done(function () {
            })
            .fail(function (res) {
                handleFail(res);
            });
    });
});

function handleFail(res) {
    if (res.status == 401) {
        window.location = "/Identity/Account/Login";
    }
    if (res.responseJSON.message) {
        alert(res.responseJSON.message);
    }
}
