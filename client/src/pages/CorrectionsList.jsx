import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const CorrectionsList = () => {
    const [corrections, setCorrections] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selectedFeedback, setSelectedFeedback] = useState(null);

    useEffect(() => {
        const fetchCorrections = async () => {
            try {
                const response = await apiClient.get('/Ertekeles');
                setCorrections(response.data);
            } catch (error) {
                console.error('Failed to fetch corrections:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchCorrections();
    }, []);

    if (loading) return <div className="page">Loading...</div>;

    return (
        <div className="page corrections-page">
            <h2>Corrections</h2>
            <div className="table-container">
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Solution ID</th>
                            <th>Score</th>
                            <th>Total Points</th>
                            <th>Feedback</th>
                            <th>Date</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {corrections.map((correction) => (
                            <tr key={correction.id}>
                                <td>{correction.id}</td>
                                <td>{correction.megoldasId}</td>
                                <td>{correction.pontszam}</td>
                                <td>{correction.osszPontszam}</td>
                                <td>
                                    <div className="feedback-cell">
                                        {correction.llmVisszajelzes?.length > 50 ? (
                                            <>
                                                {correction.llmVisszajelzes.substring(0, 50)}...
                                                <button
                                                    className="btn-link"
                                                    onClick={() => setSelectedFeedback(correction.llmVisszajelzes)}
                                                    style={{ marginLeft: '0.5rem', color: 'var(--primary-color)', background: 'none', border: 'none', cursor: 'pointer', textDecoration: 'underline' }}
                                                >
                                                    Read More
                                                </button>
                                            </>
                                        ) : (
                                            correction.llmVisszajelzes
                                        )}
                                    </div>
                                </td>
                                <td>{new Date(correction.ertekelesDatuma).toLocaleString()}</td>
                                <td>
                                    <Link to={`/solutions/${correction.megoldasId}`} className="btn btn-secondary btn-sm">
                                        View Solution
                                    </Link>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                {corrections.length === 0 && <p className="text-center mt-4">No corrections found.</p>}
            </div>

            {selectedFeedback && (
                <div className="modal-overlay" onClick={() => setSelectedFeedback(null)}>
                    <div className="modal-content" onClick={e => e.stopPropagation()}>
                        <button className="modal-close" onClick={() => setSelectedFeedback(null)}>&times;</button>
                        <h3>Feedback Details</h3>
                        <div className="modal-body">
                            {selectedFeedback}
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default CorrectionsList;
