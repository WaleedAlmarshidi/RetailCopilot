window.hideIndicator = function() {
    console.log('hideIndicator');
    var iframe = document.querySelector('#indicator'); // Corrected to select by ID
    if (iframe) {
        iframe.style.transition = 'width 1s ease, opacity 1s ease'; // Add transition properties for smooth animation
        iframe.style.opacity = 0.1; 
    }
}
window.animateInputFocus = function() {
    var iframe = document.querySelector('#indicator'); // Corrected to select by ID
    
    iframe.style.transition = 'width 1s ease, opacity 1s ease'; // Add transition property for smooth animation
    iframe.style.width = '100vw'; // Adjust the width as needed
    iframe.style.opacity = 1; 
}
window.animateInputBlur = function() {
    var iframe = document.querySelector('#indicator'); // Corrected to select by ID
    if (iframe) {
        iframe.style.transition = 'width 1s ease'; // Add transition property for smooth animation
        iframe.style.width = '50vw'; // Adjust the width as needed
    }
}