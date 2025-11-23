import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const GeneratedExamsList = () => {
    const [generatedExams, setGeneratedExams] = useState([]);
    const [baseExams, setBaseExams] = useState({});
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [genResponse, baseResponse] = await Promise.all([
                    apiClient.get(ENDPOINTS.EXAM.LIST_GENERATED),
                    apiClient.get(ENDPOINTS.EXAM.LIST)
                ]);

                setGeneratedExams(genResponse.data);

                // Create a map of base exams for easy lookup
                const baseMap = {};
                baseResponse.data.forEach(exam => {
                    baseMap[exam.id] = exam;
                });
                setBaseExams(baseMap);
            } catch (error) {
                console.error('Failed to fetch data:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    const handleGenerate = async (baseExamId) => {
        try {
            if (!baseExamId) return;
            await apiClient.post(ENDPOINTS.EXAM.GENERATE(baseExamId));
            // Refresh list
            const response = await apiClient.get(ENDPOINTS.EXAM.LIST_GENERATED);
            setGeneratedExams(response.data);
            alert('New exam instance generated successfully!');
        } catch (error) {
            console.error('Failed to generate exam:', error);
            alert('Failed to generate exam.');
        }
    };

    if (loading) return <div className="page">Loading...</div>;

    return (
        <div className="page generated-exams-page">
            <div className="header-actions">
                <h2>Generated Exams</h2>
                <div className="generate-action">
                    <select id="baseExamSelect" className="select-input">
                        <option value="">Select Base Exam to Generate</option>
                        {Object.values(baseExams).map(exam => (
                            <option key={exam.id} value={exam.id}>{exam.nev}</option>
                        ))}
                    </select>
                    <button
                        className="btn btn-primary"
                        onClick={() => handleGenerate(document.getElementById('baseExamSelect').value)}
                    >
                        Generate New Instance
                    </button>
                </div>
            </div>

            <div className="table-container">
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Base Exam Name</th>
                            <th>Generated At</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {generatedExams.map((exam) => (
                            <tr key={exam.id}>
                                <td>{exam.id}</td>
                                <td>{baseExams[exam.parentZhId]?.nev || 'Unknown'}</td>
                                <td>{new Date(exam.generationTime).toLocaleString()}</td>
                                <td>
                                    <Link to={`/generated-exams/${exam.id}/tasks`} className="btn btn-secondary btn-sm">
                                        View Tasks
                                    </Link>
                                    <Link
                                        to="/correction"
                                        state={{ generaltZhId: exam.id }}
                                        className="btn btn-primary btn-sm"
                                        style={{ marginLeft: '0.5rem' }}
                                    >
                                        Submit Solution
                                    </Link>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                {generatedExams.length === 0 && <p className="text-center mt-4">No generated exams found.</p>}
            </div>
        </div>
    );
};

export default GeneratedExamsList;
