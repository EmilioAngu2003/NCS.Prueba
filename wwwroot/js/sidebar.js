document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.getElementById('sidebar-main');
    const toggleBtn = document.getElementById('sidebar-toggle');

    toggleBtn.addEventListener('click', () => {
        if (window.innerWidth < 992) {
            const bsOffcanvas = new bootstrap.Offcanvas(sidebar);
            bsOffcanvas.show();
        } else {
            sidebar.classList.toggle('sidebar-mini');
        }
    });
});