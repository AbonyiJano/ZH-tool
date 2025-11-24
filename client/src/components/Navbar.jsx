import React from 'react';
import { Link } from 'react-router-dom';

const Navbar = () => {
    return (
        <nav className="navbar">
            <div className="navbar-brand">
                <Link to="/">ZH-Professor</Link>
            </div>
            <div className="navbar-links">
                <Link to="/" className="nav-link">Vezérlőpult</Link>
                <Link to="/uj-zh" className="nav-link">ZH sablon Létrehozása</Link>
                <Link to="/alap-zhk" className="nav-link">ZH sablonok</Link>
                <Link to="/generalt-zhk" className="nav-link">Generált ZH-k</Link>
                <Link to="/hallgatok" className="nav-link">Hallgatók</Link>
                <Link to="/javitas" className="nav-link">Javítás</Link>
                <Link to="/javitasok" className="nav-link">Megoldások Megtekintése</Link>
            </div>
        </nav>
    );
};

export default Navbar;
