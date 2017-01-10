$(document).ready(function () {
    

    $('#senden').on('click', insertItem);

});

function insertItem() {
    var $name;
    var $preis;
    var $beschreibung;
    var $pic;

    $name = $('#Name');
    $preis = $('#Preis');
    $beschreibung = $('#beschreibung');
    $pic = $('#pic');

    alert("Wird hochgeladen ...");
    var formData = new FormData();
    formData.append('name', $name.val());
    formData.append('preis', $preis.val());
    formData.append('beschreibung', $beschreibung.val())
    formData.append('file', $pic[0].files[0]);
    alert(formData.get('name'));
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
                    $('#seas').html(data);
                }
            });
        } else alert("form data undefined");
    } else alert("error length");
}