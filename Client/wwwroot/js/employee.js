
$('#insertModal').on('hidden.bs.modal', function () {
    $(this).find('form').trigger('reset');
})

$(document).ready(function () {
    let inputLeaveForm = document.getElementsByClassName("insertEmployeeForm");

    AjaxManager()
    AjaxDivisi()
    AjaxRole()
    ValidateForm(inputLeaveForm, SubmitEmployee)
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

function SubmitEmployee () {
    
    var obj = new Object(); 
    obj.name = $("#CName").val();
    obj.gender = $("#CGender").val();
    obj.email = $("#CEmail").val();
    obj.password = $("#CPassword").val();
    obj.phoneNumber = parseInt($("#CPhoneNumber").val());
    obj.role_Id = $("#CRoleId").val();
    obj.manager = $("#CManagerId").val();
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

        //Swal.fire({
        //    icon: 'success',
        //    title: 'Success',
        //    text: result.message,
        //    footer: '<a href="">Why do I have this issue?</a>'
        //})
        switch (result.errorType) {
            case 200:
                formReset();
                Swal.fire({
                    icon: 'success',
                    title: 'Successs',
                    text: result.message,
                })
                break;

            //email duplicate
            case 1:
                $(".email").val("")
                Swal.fire({
                    icon: 'error',
                    title: 'Something Wrong',
                    text: result.message,
                })
                break;

            //phone duplicate
            case 2:
                $(".phoneNumber").val("")
                Swal.fire({
                    icon: 'error',
                    title: 'Something Wrong',
                    text: result.message,
                })
                break;
            default:
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: result.message,
                })
                break;
        }
        
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
}

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
        $("#UName").val(u.name);
        if (u.gender == 0) {
            document.getElementById("gend").value = "Male";
        }
        else {
            document.getElementById("gend").value = "Female";
        }
        document.getElementById("URoleId").value = u.role_Id;
        document.getElementById("UDivisiId").value = u.divisi_id;
        document.getElementById("UManagerId").value = u.manager_id;
        $("#UPhoneNumber").val(u.phoneNumber);
        $("#UPEmail").val(u.email);
        
        console.log(u)
    })
}


$("#updateEmployee").submit(function (e) {
    e.preventDefault();
    var obj = new Object(); 
    obj.employee_id = $("#employee_id").val();
    obj.name = $("#UName").val();
    obj.gender = $("#gend").val();
    obj.phoneNumber = $("#UPhoneNumber").val();
    obj.role_Id = $("#URoleId").val();
    obj.manager_id = $("#UManagerId").val();
    obj.divisi_id = parseInt($("#UDivisiId").val());
    obj.email = $("#UPEmail").val();
    console.log(obj)

    $.ajax({
        url: "https://localhost:44302/api/Employee/",
        type: "PATCH",
        contentType: 'application/json',
        data: JSON.stringify(obj)
        //data: obj, //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
    }).done((result) => {
        $('#dataTbl').DataTable().ajax.reload();
        //$('#updateModal').modal('hide');
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Data Updated!',
            footer: '<a href="">Why do I have this issue?</a>'
        })
        console.log(result);
    }).fail((error) => {
        console.log(error)
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: '<a href="">Why do I have this issue?</a>'
        })
    })
})

function AjaxManager() {
    //let insertManagerDropDown = document.querySelector("#CManagerId")
    $.ajax({
        url: "https://localhost:44302/api/employee/allManager",
        type: "GET"
    }).done(e => {
        e.forEach(e => {
            $(".CManagerId").append(`<option value="${e.employee_id}">${e.employee_id}-${e.name}</option>`)
//            insertManagerDropDown.append(`
//                <option value="${e.employee_id}">${e.name}</option>
//`)
        })
    })
}

function AjaxDivisi() {
    //let insertManagerDropDown = document.querySelector("#CManagerId")
    $.ajax({
        url: "https://localhost:44302/api/divisi",
        type: "GET"
    }).done(e => {
        e.forEach(e => {
            $(".CDivisiId").append(`<option value="${e.divisi_id}">${e.divisi_id}-${e.namaDivisi}</option>`)
            //            insertManagerDropDown.append(`
            //                <option value="${e.employee_id}">${e.name}</option>
            //`)
        })
    })
}

function AjaxRole() {
    //let insertManagerDropDown = document.querySelector("#CManagerId")
    $.ajax({
        url: "https://localhost:44302/api/role",
        type: "GET"
    }).done(e => {
        e.forEach(e => {
            $(".CRoleId").append(`<option value="${e.role_id}">${e.roleName}</option>`)
            //            insertManagerDropDown.append(`
            //                <option value="${e.employee_id}">${e.name}</option>
            //`)
        })
    })
}