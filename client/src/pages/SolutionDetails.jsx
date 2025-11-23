import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const SolutionDetails = () => {
    const { id } = useParams();
    const [solution, setSolution] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchSolution = async () => {
            try {
                const response = await apiClient.get(ENDPOINTS.SOLUTION.GET_BY_ID(id));
                setSolution(response.data);
            } catch (error) {
                console.error('Failed to fetch solution:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchSolution();
    }, [id]);

    if (loading) return <div className="page">Loading...</div>;
    if (!solution) return <div className="page">Solution not found.</div>;

    return (
        <div className="page solution-details-page">
            <div className="header-actions">
                <h2>Solution Details #{solution.id}</h2>
                <div style={{ display: 'flex', gap: '1rem' }}>
                    <Link to={`/generated-exams/${solution.generaltZhId}/tasks`} className="btn btn-primary">
                        View Exam Tasks
                    </Link>
                    <Link to="/corrections" className="btn btn-secondary">Back to Corrections</Link>
                </div>
            </div>

            <div className="card">
                <div className="form-group">
                    <label>Student Neptun Code</label>
                    <div className="static-value">{solution.hallgatoNeptunkod}</div>
                </div>
                <div className="form-group">
                    <label>Generated Exam ID</label>
                    <div className="static-value">{solution.generaltZhId}</div>
                </div>
                <div className="form-group">
                    <label>Submitted Solution</label>
                    <pre className="code-block">{solution.bekuldottMegoldas}</pre>
                </div>
                <div className="form-group">
                    <label>Submission Date</label>
                    <div className="static-value">{new Date().toLocaleString()}</div>
                </div>
            </div>
        </div>
    );
};

export default SolutionDetails;
