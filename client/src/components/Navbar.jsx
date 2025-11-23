import React from 'react';
import { Link } from 'react-router-dom';

const Navbar = () => {
    return (
        <nav className="navbar">
            <div className="navbar-brand">
                <Link to="/">ZH-Tool</Link>
            </div>
            <div className="navbar-links">
                <Link to="/" className="nav-link">Dashboard</Link>
                <Link to="/create-exam" className="nav-link">Create Exam</Link>
                <Link to="/base-exams" className="nav-link">Base Exams</Link>
                <Link to="/students" className="nav-link">Students</Link>
                <Link to="/correction" className="nav-link">Correction</Link>
                <Link to="/generated-exams" className="nav-link">Generated Exams</Link>
                <Link to="/corrections" className="nav-link">View Corrections</Link>
            </div>
        </nav>
    );
};

export default Navbar;
