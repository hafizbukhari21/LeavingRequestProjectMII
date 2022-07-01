$(document).ready(function () {
    $('#dataTbl').DataTable({
        "ajax": {
            "url": "https://localhost:44302/api/employee",
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
                "data": null,
                "sortable": true,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
            },
            {
                "data": "name"
            },
            {
                "data": "email"
            },
            {
                "data": "phoneNumber"
            },
            {
                "data": "roleName"
            },
            {
                "data": "namaDivisi"
            },
            {
                data: "employee_id",
                render: function (data, type, row, meta) {
                    return `<div class="btn-group text-center">
                                <button type="button" class="btn btn-sm btn-warning" title="Detail" onClick="employeeDetail('https://localhost:44302/api/employee/${data}')" data-toggle="modal" data-target="#updateModal">
                                <i class="fas fa-edit"></i>
                                </button>
                                <button type="button" class="btn btn-sm btn-danger" title="Delete" onClick="employeeDelete('${data}')" >
                                <i class="fas fa-trash"></i>
                                </button>
                            </div>`
                }
            }
        ]
    });
});

$("#insertEmployee").submit(function (e) {
    e.preventDefault();
    var obj = new Object(); 
    obj.name = $("#CName").val();
    obj.gender = $("#CGender").val();
    obj.email = $("#CEmail").val();
    obj.password = $("#CPassword").val();
    obj.phoneNumber = parseInt($("#CPhoneNumber").val());
    obj.role_Id = $("#CRoleId").val();
    obj.manager_Id = $("#CManagerId").val();
    obj.divisi_Id = $("#CDivisiId").val();
    console.log(obj);
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: "https://localhost:44302/api/employee",
        type: "POST",
        contentType: 'application/json',
        //data: obj,
        data: JSON.stringify(obj), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
    }).done((result) => {
        console.log(result)
        $('#dataTbl').DataTable().ajax.reload();
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: result.message,
            footer: '<a href="">Why do I have this issue?</a>'
        })
        formReset();
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
    document.getElementById("insertEmployee").reset();
    $('#btnInput').prop("disabled", false);
}

function employeeDelete(empId) {

    let data = {
        employee_id : empId
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
                    url: "https://localhost:44302/api/employee",
                    data: JSON.stringify(data),
                    type: "DELETE",
                    contentType: 'application/json',
                    //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
                }).done((result) => {
                    //alert('Sukses');
                    $('#dataTbl').DataTable().ajax.reload();
                }).fail((error) => {
                    console.log(error)
                })
        } else if (result.dismiss === Swal.DismissReason.cancel)
        {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your imaginary file is safe :)',
                'error'
            )
        }
    })
}

function employeeDetail(urlEmp) {
    $.ajax({
        url: urlEmp
    }).done(u => {
        $("#employee_id").val(u.employee_id);
        $("#updateModal #CName").val(u.name);
        if (u.gender == 0) {
            document.getElementById("gend").value = "Male";
        }
        else {
            document.getElementById("gend").value = "Female";
        }
        $("#updateModal #CEmail").val(u.email);
        $("#updateModal #USisaCuti").val(u.sisaCuti);
        $("#updateModal #CPhoneNumber").val(u.phoneNumber);
        
        console.log(u)
    })
}