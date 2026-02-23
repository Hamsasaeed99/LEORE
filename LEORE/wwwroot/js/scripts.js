document.addEventListener("DOMContentLoaded", () => {
  const links = document.querySelectorAll(".nav-link");
  if (!links.length) return;

  // Restore from localStorage or default to Home
  const saved = localStorage.getItem("activeNav");
  if (saved) setActive(saved);
  else setActive("Home");

  links.forEach((a) => {
    a.addEventListener("click", (e) => {
      // Allow normal navigation if href is not '#'
      const href = a.getAttribute("href");
      const title = a.textContent.trim();
      setActive(title);
      // Persist selection
      localStorage.setItem("activeNav", title);

      if (href === "#" || href === "") {
        // prevent default for demo anchors so the page doesn't jump
        e.preventDefault();
      }
    });
  });

  function setActive(name) {
    links.forEach((a) => {
      const title = a.textContent.trim();
      if (title === name) a.classList.add("active");
      else a.classList.remove("active");
    });
  }
});
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