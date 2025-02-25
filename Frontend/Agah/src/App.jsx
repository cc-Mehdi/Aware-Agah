import Home from './Pages/Home'
import ErrorHandler from "./components/ErrorHandler";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from './pages/Login';
import NotFound from './pages/NotFound';
import { useEffect } from 'react';


function App() {
    useEffect(() => {
        const link = document.createElement('link');
        link.href = 'https://fonts.googleapis.com/css2?family=Vazirmatn:wght@400;700&display=swap';
        link.rel = 'stylesheet';
        document.head.appendChild(link);
        document.body.style.fontFamily = 'Vazirmatn, sans-serif';
    }, []);
    return (
        <>
            <ErrorHandler />
            <Router>
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/Login" element={<Login />} />
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </Router>
        </>
    )
}

export default App