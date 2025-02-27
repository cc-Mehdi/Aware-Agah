export const api_HandleLogin = async (email, password) => {
    try {
        const response = await fetch('https://localhost:44314/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email, password }),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        localStorage.setItem('token', data.token);
        return data; // Return the response data
    } catch (error) {
        console.error('Login failed', error);
        throw error; // Re-throw the error if needed
    }
};