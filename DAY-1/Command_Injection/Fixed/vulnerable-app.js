const express = require('express');
const { exec } = require('child_process');

const app = express();
app.use(express.urlencoded({ extended: true }));

// Function to validate hostname or IP address
function isValidHost(host) {
    // Regular expression for matching valid hostnames or IP addresses
    const regex = /^(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}$|^(?:\d{1,3}\.){3}\d{1,3}$/;
    return regex.test(host);
}

app.get('/', (req, res) => {
    res.send(`
        <h1>Ping a Website</h1>
        <form method="POST" action="/ping">
            <label for="host">Enter a hostname or IP address:</label>
            <input type="text" id="host" name="host" placeholder="e.g., google.com">
            <button type="submit">Ping</button>
        </form>
    `);
});

app.post('/ping', (req, res) => {
    const host = req.body.host;

    // Validate the input to prevent command injection
    if (!host || !isValidHost(host)) {
        return res.send('Invalid hostname or IP address. Please provide a valid input.');
    }

    // Escape user input to prevent command injection
    const sanitizedHost = host.replace(/[^a-zA-Z0-9.-]/g, '');
  
    // Execute ping command safely
    exec(`ping -c 4 ${sanitizedHost}`, (error, stdout, stderr) => {
        if (error) {
            return res.send(`Error: ${stderr}`);
        }
        res.send(`<pre>${stdout}</pre>`);
    });
});

app.listen(3000, () => {
    console.log('Ping app running on http://localhost:3000');
});
