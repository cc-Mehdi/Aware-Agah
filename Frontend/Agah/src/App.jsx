import Home from './Pages/Home'
import ErrorHandler from "./components/ErrorHandler";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from './pages/Login';
import NotFound from './pages/NotFound';
import { useEffect } from 'react';
import ProtectedRoute from './components/ProtectedRoute';


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
                    <Route path="/Login" element={<Login />} />
                    <Route element={<ProtectedRoute />}>
                        <Route path="/" element={<Home />} />
                    </Route>
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </Router>
        </>
    )
}

export default App