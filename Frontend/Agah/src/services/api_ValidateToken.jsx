export const validateToken = async () => {
    const token = localStorage.getItem("token");
    if (!token) return false;

    try {
        const response = await fetch("https://localhost:44314/api/Auth/ValidateToken", {
            headers: { Authorization: `Bearer ${token}` },
        });

        if (!response.ok) throw new Error("Invalid token");

        return true;
    } catch (error) {
        return false;
    }
};