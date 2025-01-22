// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function() {
    const categoryMenuBtn = document.getElementById('categoryMenuBtn');
    const categoryDropdown = document.getElementById('categoryDropdown');

    // Toggle dropdown when clicking the button
    categoryMenuBtn.addEventListener('click', function(e) {
        e.stopPropagation();
        categoryDropdown.classList.toggle('hidden');
    });

    // Hide dropdown when clicking outside
    document.addEventListener('click', function(e) {
        if (!categoryDropdown.contains(e.target) && !categoryMenuBtn.contains(e.target)) {
            categoryDropdown.classList.add('hidden');
        }
    });
});

document.addEventListener('DOMContentLoaded', function() {
    const categoryMenuBtn = document.getElementById('categoryMenuBtn');
    const categoryDropdown = document.getElementById('categoryDropdown');
    let isHovering = false;

    categoryMenuBtn.addEventListener('mouseenter', function() {
        categoryDropdown.classList.remove('hidden');
    });

    categoryDropdown.addEventListener('mouseenter', function() {
        isHovering = true;
    });

    categoryDropdown.addEventListener('mouseleave', function() {
        isHovering = false;
        categoryDropdown.classList.add('hidden');
    });

    categoryMenuBtn.addEventListener('mouseleave', function() {
        setTimeout(() => {
            if (!isHovering) {
                categoryDropdown.classList.add('hidden');
            }
        }, 100);
    });
});