$("input[value=-1]").on("click", function () {
    $(this).closest(".formCheckboxList").find("input[type=checkbox]").prop("checked", $(this).prop("checked"));
});

$(".formCheckboxList").find("input[type=checkbox]").on("click", function () {
    if (!$(this).prop("checked")) {
        $(this).closest(".formCheckboxList").find("input[value=-1]").prop("checked", false);
    }
});
