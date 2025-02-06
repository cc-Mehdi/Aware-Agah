export const getProductNames = async () => {
    try {
        const response = await fetch('https://localhost:44314/api/Product/GetProducts', {
            method: 'Get',
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        return data.result;
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};