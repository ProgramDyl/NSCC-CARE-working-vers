import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Card from 'react-bootstrap/Card';
import axios from 'axios';
import AssessmentsCard from '../components/profile-components/AssessmentsCard';

const PatientElimination = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [answers, setAnswers] = useState({});
    const [initialAnswers, setInitialAnswers] = useState({});

    const APIHOST = import.meta.env.VITE_API_URL;

    useEffect(() => {
        const savedData = localStorage.getItem(`patient-elimination-${id}`);
        if (savedData) {
            const parsed = JSON.parse(savedData);
            setAnswers(parsed);
            setInitialAnswers(parsed);
        } else {
            const today = new Date().toISOString().split('T')[0];
            const defaultState = {
                catheterInsertionDate: today
            };
            setAnswers(defaultState);
            setInitialAnswers(defaultState);
            fetchPatientData();
        }
    }, [id]);

    const fetchPatientData = async () => {
        try {
            const response = await axios.get(`${APIHOST}/api/patients/nurse/patient/${id}/elimination`);
            setAnswers(prev => ({ ...prev, ...response.data }));
            setInitialAnswers(prev => ({ ...prev, ...response.data }));
        } catch (error) {
            console.error('Error fetching patient:', error);
        }
    };

    const handleAnswerChange = (question, answer) => {
        setAnswers(prev => {
            const updated = { ...prev, [question]: answer };

            // Auto-fill today's date if catheter insertion is set to "yes" and no date exists
            if (
                question === 'catheterInsertion' &&
                answer === 'yes' &&
                !prev.catheterInsertionDate
            ) {
                updated.catheterInsertionDate = new Date().toISOString().split('T')[0];
            }

            return updated;
        });
    };

    const handleSave = () => {
        try {
            localStorage.setItem(`patient-elimination-${id}`, JSON.stringify(answers));
            setInitialAnswers(answers);
            alert('Elimination data saved successfully!');
        } catch (error) {
            console.error('Error saving data:', error);
            alert('Failed to save data. Please try again.');
        }
    };

    const isDirty = () => {
        return JSON.stringify(answers) !== JSON.stringify(initialAnswers);
    };

    return (
        <div className="container mt-4 d-flex">
            <AssessmentsCard />
            <div className="ms-4 flex-fill">
                <div className="d-flex justify-content-between align-items-center mb-4">
                    <h2>Elimination</h2>
                    <div className="d-flex gap-2">
                        <Button variant="primary" onClick={() => navigate(`/api/patients/${id}`)}>
                            Go Back to Profile
                        </Button>
                        <Button
                            onClick={handleSave}
                            disabled={!isDirty()}
                            variant={isDirty() ? 'success' : 'secondary'}
                            style={{
                                opacity: isDirty() ? 1 : 0.5,
                                cursor: isDirty() ? 'pointer' : 'not-allowed',
                                border: 'none',
                                backgroundColor: isDirty() ? '#198754' : '#e0e0e0',
                                color: isDirty() ? 'white' : '#777',
                                pointerEvents: isDirty() ? 'auto' : 'none'
                            }}
                        >
                            {isDirty() ? 'Save' : 'No Changes'}
                        </Button>
                    </div>
                </div>

                {/* Day/Night Product */}
                <Card className="mt-4">
                    <Card.Body>
                        <Form>
                            <Form.Group className="mb-3 d-flex align-items-center">
                                <Form.Label className="me-3 mb-0">Day/Night Product</Form.Label>
                                <div className="ms-auto d-flex align-items-center">
                                    <Form.Check
                                        inline
                                        name="dayOrNightProduct"
                                        type="radio"
                                        id="dayOrNightProduct-yes"
                                        label="Yes"
                                        checked={answers.dayOrNightProduct === 'yes'}
                                        onChange={() => handleAnswerChange('dayOrNightProduct', 'yes')}
                                    />
                                    <Form.Check
                                        inline
                                        name="dayOrNightProduct"
                                        type="radio"
                                        id="dayOrNightProduct-no"
                                        label="No"
                                        checked={answers.dayOrNightProduct === 'no'}
                                        onChange={() => handleAnswerChange('dayOrNightProduct', 'no')}
                                    />
                                </div>
                            </Form.Group>

                            {answers.dayOrNightProduct === 'yes' && (
                                <Form.Group className="mb-3">
                                    <div className="d-flex">
                                        <Form.Check
                                            inline
                                            type="checkbox"
                                            id="dayProduct"
                                            label="Day"
                                            checked={answers.dayProduct}
                                            onChange={() =>
                                                handleAnswerChange('dayProduct', !answers.dayProduct)
                                            }
                                        />
                                        <Form.Check
                                            inline
                                            type="checkbox"
                                            id="nightProduct"
                                            label="Night"
                                            checked={answers.nightProduct}
                                            onChange={() =>
                                                handleAnswerChange('nightProduct', !answers.nightProduct)
                                            }
                                        />
                                    </div>
                                </Form.Group>
                            )}
                        </Form>
                    </Card.Body>
                </Card>

                {/* Catheter Insertion + Date */}
                <Card className="mt-4">
                    <Card.Body>
                        <Form>
                            <Form.Group className="mb-3 d-flex align-items-center">
                                <Form.Label className="me-3 mb-0">Catheter Insertion</Form.Label>
                                <div className="ms-auto d-flex align-items-center">
                                    <Form.Check
                                        inline
                                        name="catheterInsertion"
                                        type="radio"
                                        id="catheterInsertion-yes"
                                        label="Yes"
                                        checked={answers.catheterInsertion === 'yes'}
                                        onChange={() => handleAnswerChange('catheterInsertion', 'yes')}
                                    />
                                    <Form.Check
                                        inline
                                        name="catheterInsertion"
                                        type="radio"
                                        id="catheterInsertion-no"
                                        label="No"
                                        checked={answers.catheterInsertion === 'no'}
                                        onChange={() => handleAnswerChange('catheterInsertion', 'no')}
                                    />
                                </div>
                            </Form.Group>

                            {answers.catheterInsertion === 'yes' && (
                                <Form.Group className="mb-3">
                                    <Form.Label>Catheter Insertion Date</Form.Label>
                                    <Form.Control
                                        type="date"
                                        value={answers.catheterInsertionDate || ''}
                                        onChange={(e) =>
                                            handleAnswerChange('catheterInsertionDate', e.target.value)
                                        }
                                        style={{ maxWidth: '200px' }}
                                    />
                                </Form.Group>
                            )}
                        </Form>
                    </Card.Body>
                </Card>

                {/* Elimination Routine */}
                <Card className="mt-4">
                    <Card.Body>
                        <Form>
                            <Form.Group className="mb-3">
                                <Form.Label>Elimination Routine</Form.Label>
                                <Form.Control
                                    type="text"
                                    value={answers.eliminationRoutine || ''}
                                    onChange={(e) => handleAnswerChange('eliminationRoutine', e.target.value)}
                                />
                            </Form.Group>
                        </Form>
                    </Card.Body>
                </Card>
            </div>
        </div>
    );
};

export default PatientElimination;
