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
window.animateInputShake = function() {
    var input = document.getElementById('contactName');  // Select the input by ID
    console.log('before if animateInputShake');
    if (input) {
        // Ensure the transition is set up for transform
        input.style.transition = 'transform 0.09s';

        // Move input to the left
        input.style.transform = 'translateX(-10px)';

        // After 0.5 seconds, move input to the right
        setTimeout(() => {
            input.style.transform = 'translateX(20px)';

            // After another 0.5 seconds, move input back to the left
            setTimeout(() => {
                input.style.transform = 'translateX(-10px)';
            }, 100);
        }, 100);

        console.log('animateInputShake');
        // Listener to remove the class after animation ends
        // input.addEventListener('animationend', function() {
        //     input.classList.remove('shaking');
        // }, { once: true });
    }
}