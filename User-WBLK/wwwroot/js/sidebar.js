document.addEventListener('DOMContentLoaded', function() {
    const sidebar = document.getElementById('mobileSidebar');
    const overlay = document.getElementById('sidebarOverlay');
    
    document.getElementById('mobileMenuBtn').addEventListener('click', function() {
        sidebar.classList.remove('-translate-x-full');
        overlay.classList.remove('opacity-0', 'pointer-events-none');
        overlay.classList.add('opacity-50');
    });

    function closeSidebar() {
        sidebar.classList.add('-translate-x-full');
        overlay.classList.remove('opacity-50');
        overlay.classList.add('opacity-0', 'pointer-events-none');
    }

    document.getElementById('closeSidebarBtn').addEventListener('click', closeSidebar);
    overlay.addEventListener('click', closeSidebar);
}); 