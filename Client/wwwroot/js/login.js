$('#formLogin').submit(function (e) {
    e.preventDefault();
    var obj = new Object();
    obj.Email = $("#LoginEmail").val();
    obj.Password = $("#LoginPassword").val();
    console.log(obj);
    $.ajax({
        url: "Login/Auth",
        type: "POST",
        data: obj,
    }).done((result) => {
        console.log(result)
        Swal.fire({
            title: 'Please Wait !!!',
            html: `<div class="spinner-grow text-primary" role="status">
                              <span class="sr-only">Loading...</span>
                            </div>
                            <div class="spinner-grow text-secondary" role="status">
                              <span class="sr-only">Loading...</span>
                            </div>
                            <div class="spinner-grow text-success" role="status">
                              <span class="sr-only">Loading...</span>
                            </div>
                            <div class="spinner-grow text-danger" role="status">
                              <span class="sr-only">Loading...</span>
                            </div>
                            <div class="spinner-grow text-warning" role="status">
                              <span class="sr-only">Loading...</span>
                            </div>`,
            allowOutsideClick: false,
            onBeforeOpen: () => {
                Swal.showLoading()
            },
            showConfirmButton: false,
        });
        switch (result.errorType) {
            case 200:
                window.location.replace("../login")
                break;
            case 3:
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
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
        console.log(error)
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: error.meesage,
        })
    })
})