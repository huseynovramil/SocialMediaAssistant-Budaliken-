$(document).ready(function () {
    let selected = $('#pointsSelected');
    let account_points = $('#account_points');
    let add_account_button = $('#addAccount');
    $('#points .dropdown-item').click(function () {
        let val = $(this).text();
        selected.val(val);
        selected.text(val);
        account_points.val(val);
        add_account_button.removeAttr('disabled');
    });
});