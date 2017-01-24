$(document).ready(function () {
    

    $('#senden').on('click', insertItem);
    $('#speichern').on('click', updateItem);

});
function updateItem() {
    var $name;
    var $preis;
    var $beschreibung;
    var $pic;
    var $prozent = $('#seas');

    $name = $('#Name');
    $preis = $('#Preis');
    $beschreibung = $('#beschreibung');
    $pic = $('#pic');

    var formData = new FormData();
    formData.append('name', $name.val());
    formData.append('preis', $preis.val());
    formData.append('beschreibung', $beschreibung.val())
    formData.append('file', $pic[0].files[0]);
    if ($pic.val() !== '') {
        if (window.FormData !== undefined) {
            console.log(formData);
            $.ajax({
                type: 'POST',
                url: 'UpdateItemAjax',
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data == 'ok') {
                        $('#senden').attr('disabled', 'disabled')
                        $('#seas').html('Das Produkt wurde erfolgreich hinzugefügt!');
                    }

                }
            });
        } else alert("form data undefined");
    } else alert("error length");
}


function insertItem() {
    var $name;
    var $preis;
    var $beschreibung;
    var $pic;
    var $prozent = $('#seas');

    $name = $('#Name');
    $preis = $('#Preis');
    $beschreibung = $('#beschreibung');
    $pic = $('#pic');

    var formData = new FormData();
    formData.append('name', $name.val());
    formData.append('preis', $preis.val());
    formData.append('beschreibung', $beschreibung.val())
    formData.append('file', $pic[0].files[0]);
    if ($pic.val() !== '' ) {
        if (window.FormData !== undefined) {            
            console.log(formData);
            $.ajax({
                type: 'POST',
                url: 'AddItemAjax',
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data == 'ok') {
                        $('#senden').attr('disabled', 'disabled')
                        $('#seas').html('Das Produkt wurde erfolgreich hinzugefügt!');
                    }
                   
                }
            });
        } else alert("form data undefined");
    } else alert("error length");
}