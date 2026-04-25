// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Auto-hide toast messages
document.addEventListener('DOMContentLoaded', function () {
    const toasts = document.querySelectorAll('.toast');
    toasts.forEach(toast => {
        setTimeout(() => {
            toast.style.opacity = '0';
            toast.style.transition = 'opacity 0.3s';
            setTimeout(() => toast.remove(), 300);
        }, 4000);
    });
});

// Drag and drop support for upload area
const uploadArea = document.querySelector('.upload-area');
if (uploadArea) {
    uploadArea.addEventListener('dragover', (e) => {
        e.preventDefault();
        uploadArea.style.borderColor = '#3b82f6';
        uploadArea.style.background = '#1e293bcc';
    });

    uploadArea.addEventListener('dragleave', () => {
        uploadArea.style.borderColor = '#475569';
        uploadArea.style.background = '#1e293b';
    });

    uploadArea.addEventListener('drop', (e) => {
        e.preventDefault();
        uploadArea.style.borderColor = '#475569';
        uploadArea.style.background = '#1e293b';

        const files = e.dataTransfer.files;
        if (files.length > 0) {
            const fileInput = document.getElementById('fileUpload');
            fileInput.files = files;
            fileInput.dispatchEvent(new Event('change'));
        }
    });
}