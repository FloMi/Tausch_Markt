$(document).ready(function () {
    var $name;
    var $preis;
    var $beschreibung;
    var $pic;

    $name = $('#Name');
    $preis = $('#Preis');
    $beschreibung = $('#beschreibung');
    $pic = $('#pic');

    $('#senden').on('click', insertItem);

});

function insertItem() {
    alert("Wird hochgeladen ...");
    var formData = new FormData($('#add-form'));
    alert(JSON.stringify(formData));
    if (formData["file"].length > 0) {
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (var x = 0; x < pic.length; x++) {
                data.append("file" + x, pic[x]);
            }
            $.post({
                url: 'Product/AddItemAjax',
                data: formData,
                beforeSend: function (data) { $('#seas').html("Bitte warten ..."); },
                success: function (data) {
                    $('#seas').html(data);
                }
            });
        } else alert("form data undefined");
    } else alert("error length");
}