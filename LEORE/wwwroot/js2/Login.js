// Handle form submission
document.getElementById('loginForm').addEventListener('submit', function(e) {
    e.preventDefault();
    
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const rememberMe = document.getElementById('rememberMe').checked;
    
    // Here you would typically send this data to a server
    console.log('Login attempt:', { email, password, rememberMe });
    
    // For demo purposes, just show an alert
    alert('Login functionality would be implemented here. Email: ' + email);
});

