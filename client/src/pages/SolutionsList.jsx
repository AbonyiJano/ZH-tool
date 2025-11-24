import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const SolutionsList = () => {
    const [solutions, setSolutions] = useState([]);
    const [students, setStudents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selectedStudent, setSelectedStudent] = useState('');

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [solutionsRes, studentsRes] = await Promise.all([
                    apiClient.get('/Ertekeles'),
                    apiClient.get(ENDPOINTS.STUDENT.LIST)
                ]);

                // Fetch all solutions using Megoldas endpoint
                const allSolutionsRes = await apiClient.get('/Megoldas');

                // Map ertekelesek with solution data
                const ertekelesMap = new Map(solutionsRes.data.map(e => [e.megoldasId, e]));
                const enrichedSolutions = allSolutionsRes.data.map(m => ({
                    ...m,
                    ertekeles: ertekelesMap.get(m.id) || null
                }));

                setSolutions(enrichedSolutions);
                setStudents(studentsRes.data);
            } catch (error) {
                console.error('Failed to fetch data:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    const filteredSolutions = selectedStudent
        ? solutions.filter(s => s.hallgatoNeptunkod === selectedStudent)
        : solutions;

    if (loading) return <div className="page">Betöltés...</div>;

    return (
        <div className="page solutions-page">
            <h2>Megoldások</h2>

            <div className="form-group" style={{ marginBottom: '1.5rem', maxWidth: '300px' }}>
                <label>Szűrés Hallgató Szerint</label>
                <select
                    value={selectedStudent}
                    onChange={(e) => setSelectedStudent(e.target.value)}
                    className="select-input"
                >
                    <option value="">Összes Hallgató</option>
                    {students.map(student => (
                        <option key={student.neptunkod} value={student.neptunkod}>
                            {student.nev} ({student.neptunkod})
                        </option>
                    ))}
                </select>
            </div>

            <div className="table-container">
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>Megoldás Azonosító</th>
                            <th>Hallgató</th>
                            <th>Generált ZH</th>
                            <th>Beküldés Dátuma</th>
                            <th>Pontszám</th>
                            <th>Műveletek</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredSolutions.map((solution) => {
                            const student = students.find(s => s.neptunkod === solution.hallgatoNeptunkod);
                            return (
                                <tr key={solution.id}>
                                    <td>{solution.id}</td>
                                    <td>{student ? `${student.nev} (${student.neptunkod})` : solution.hallgatoNeptunkod}</td>
                                    <td>{solution.generaltZhId}</td>
                                    <td>{new Date(solution.bekuldesDatuma).toLocaleString()}</td>
                                    <td>
                                        {solution.ertekeles
                                            ? `${solution.ertekeles.pontszam}/${solution.ertekeles.osszPontszam}`
                                            : 'Még nincs értékelve'
                                        }
                                    </td>
                                    <td>
                                        <Link to={`/megoldasok/${solution.id}`} className="btn btn-secondary btn-sm">
                                            Megoldás Megtekintése
                                        </Link>
                                    </td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
                {filteredSolutions.length === 0 && <p className="text-center mt-4">Nincsenek megoldások.</p>}
            </div>
        </div>
    );
};

export default SolutionsList;
