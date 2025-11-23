import React from 'react';
import { Link } from 'react-router-dom';

const Home = () => {
    return (
        <div className="page home-page">
            <h1>Teacher Dashboard</h1>
            <p>Welcome to the ZH-Tool Teacher Interface.</p>
            <div className="dashboard-grid">
                <Link to="/create-exam" className="card dashboard-card">
                    <h3>Create Exam</h3>
                    <p>Design new exams and tasks.</p>
                </Link>
                <Link to="/students" className="card dashboard-card">
                    <h3>Manage Students</h3>
                    <p>Register and view students.</p>
                </Link>
                <Link to="/correction" className="card dashboard-card">
                    <h3>Correction</h3>
                    <p>Submit and correct student solutions.</p>
                </Link>
            </div>
        </div>
    );
};

export default Home;
