
$('#insertModal').on('hidden.bs.modal', function () {
    $(this).find('form').trigger('reset');
})

$(document).ready(function () {
    var table = $('#dataTbl').DataTable({
        "ajax": {
            "url": "https://localhost:44302/api/category",
            "dataType": "json",
            "dataSrc": "",
        },
        dom: 'lBfrtip',
        buttons: [
            {
                extend: 'copy',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-copy"> </i>',
                titleAttr: 'Copy to clipboard'
            },
            {
                extend: 'excel',
                className: 'btn btn-success btn-sm',
                text: '<i class="fa fa-file-excel"> </i>',
                titleAttr: 'Download to excel'
            },
            {
                extend: 'pdf',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-file-pdf"> </i>',
                titleAttr: 'Download to pdf'
            },
            {
                extend: 'print',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-print"> </i>',
                titleAttr: 'Print this table'
            },
            {
                html: `<!-- Button trigger modal -->
                <button title="Add new data" type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#insertModal">
                    <i class="fa fa-plus"></i>
                </button>`
            }
        ],
        initComplete: function () {
            var btns = $('.dt-button');
            btns.removeClass('dt-button');
        },
        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            {
                "data": "nameCategory"
            },
        ]
    });
    $('#dataTbl tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            row.child(format(row.data())).show();
            tr.addClass('shown')
        }
    });
});

function format(d) {
    console.log(d)
    return `
            <table cellpadding="5" cellspacing="0" style="padding-left:50px;">
                <tr>
                    <td>Id</td>
                    <td>:</td>
                    <td id="divisi_id">${d.category_id}</td>
                </tr>
                <tr>
                    <td>Name</td>
                    <td>:</td>
                    <input required type="hidden" class="form-control" id="category_ids" value="${d.category_id}">
                    <td><input required type="text" class="form-control" id="nameCategory" placeholder="Enter Category Name" value="${d.nameCategory}"></td>
                    <td>
                        <button type="submit" class="btn btn-primary" id="btnUpdate" onclick="updateCategory()"><i class="fas fa-save"></i> Save Changes</button>
                    </td>
                </tr>
            </table>
            `
}

function divisiDetail(urlDiv) {
    $.ajax({
        url: urlDiv
    }).done(u => {
        $("#category_id").val(u.category_id);
        $("#nameCategory").val(u.nameCategory);
        console.log(u)
    })
}

function updateCategory() {
    var obj = new Object();
    obj.category_id = parseInt($("#category_ids").val());;
    obj.nameCategory = $("#nameCategory").val();
    console.log(obj)
    $.ajax({
        url: "https://localhost:44302/api/category/",
        type: "PATCH",
        contentType: 'application/json',
        data: JSON.stringify(obj)
    }).done((result) => {
        $('#dataTbl').DataTable().ajax.reload();
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Data Updated!',
        })
        console.log(result);
    }).fail((error) => {
        console.log(error)
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
        })
    })
}

$("#insertCategory").submit(function (e) {
    e.preventDefault();
    var obj = new Object();
    obj.nameCategory = $("#nameCategory").val();
    $.ajax({
        url: "https://localhost:44302/api/category",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(obj), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
    }).done((result) => {
        $("#insertCategory").toggleClass('was-validated needs-validation');
        console.log(result)
        $('#dataTbl').DataTable().ajax.reload();
        formReset();
        Swal.fire({
            icon: 'success',
            title: 'Successs',
            text: result.message,
        })
    }).fail((error) => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: "Mohon ini semua field dengan benar !!",
        })
        console.log(error)
    })
})

function formReset() {
    document.getElementById("insertCategory").reset();
    $('#btnInput').prop("disabled", false);
}
