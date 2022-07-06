
$('#insertModal').on('hidden.bs.modal', function () {
    $(this).find('form').trigger('reset');
})

$(document).ready(function () {
    var table = $('#dataTbl').DataTable({
        "ajax": {
            "url": "https://localhost:44302/api/divisi",
            "dataType": "json",
            "dataSrc": "",
        },
        dom: 'lBfrtip',
        buttons: [
            //'copy', 'csv', 'excel', 'pdf', 'print'
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
            //btns.addClass('btn btn-primary btn-sm');
            btns.removeClass('dt-button');
        },
        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            /*{
                "data": "divisi_id"
            },*/
            {
                "data": "namaDivisi"
            },
            /*{
                data: "divisi_id",
                render: function (data, type, row, meta) {
                    return `<div class="btn-group text-center">
                                <button type="button" class="btn btn-sm btn-danger" title="Delete" onClick="divisiDelete('${data}')" >
                                <i class="fas fa-trash"></i>
                                </button>
                            </div>`
                }
            }*/
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
                    <td id="divisi_id">${d.divisi_id}</td>
                </tr>
                <tr>
                    <td>Name</td>
                    <td>:</td>
                    <input required type="hidden" class="form-control" id="divisi_ids" value="${d.divisi_id}">
                    <td><input required type="text" class="form-control" id="namaDivisi" placeholder="Enter Division Name" value="${d.namaDivisi}"></td>
                    <td>
                        <button type="submit" class="btn btn-primary" id="btnUpdate" onclick="updateDiv()"><i class="fas fa-save"></i> Save Changes</button>
                        <button type="button" class="btn btn-danger" title="Delete" onClick="divisiDelete('${d.divisi_id}')" >
                                <i class="fas fa-trash"></i> Delete
                        </button>
                    </td>
                </tr>
            </table>
            `
}

function divisiDetail(urlDiv) {
    $.ajax({
        url: urlDiv
    }).done(u => {
        $("#divisi_id").val(u.divisi_id);
        $("#namaDivisi").val(u.namaDivisi);

        console.log(u)
    })
}

function updateDiv() {
    var obj = new Object();
    obj.divisi_id = parseInt($("#divisi_ids").val());;
    obj.namaDivisi = $("#namaDivisi").val();
    console.log(obj)
    $.ajax({
        url: "https://localhost:44302/api/Divisi/",
        type: "PATCH",
        contentType: 'application/json',
        data: JSON.stringify(obj)
    }).done((result) => {
        $('#dataTbl').DataTable().ajax.reload();
        //$('#updateModal').modal('hide');
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

$("#insertDivisi").submit(function (e) {
    e.preventDefault();
    var obj = new Object();
    obj.namaDivisi = $("#namaDivisi").val();
    $.ajax({
        url: "https://localhost:44302/api/divisi",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(obj), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
    }).done((result) => {
        console.log(result)
        $('#dataTbl').DataTable().ajax.reload();
        formReset();
        //$('#insertDivisi').modal('hide');
        Swal.fire({
            icon: 'success',
            title: 'Successs',
            text: result.message,
        })
    }).fail((error) => {
        switch (error.errorType) {
            case 1:
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: error.message,
                })
                break;
            case 2:
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: error.message,
                })
                break;
            default:
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: "Mohon ini semua field dengan benar !!",
                })
                break;
        }
        console.log(error)
    })
})

function formReset() {
    document.getElementById("insertDivisi").reset();
    $('#btnInput').prop("disabled", false);
}

function divisiDelete(divId) {

    let data = {
        divisi_id: divId
    }
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })
    //sweet altert
    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            swalWithBootstrapButtons.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            ),
                $.ajax({
                    url: "https://localhost:44302/api/divisi",
                    data: JSON.stringify(data),
                    type: "DELETE",
                    contentType: 'application/json',
                    //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
                }).done((result) => {
                    //console.log()
                    //alert('Sukses');
                    $('#dataTbl').DataTable().ajax.reload();
                }).fail((error) => {
                    console.log(error)
                })
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your imaginary file is safe :)',
                'error'
            )
        }
    })
}