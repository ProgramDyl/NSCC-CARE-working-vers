import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import "../css/home_styles.css"
import logo from "../img/CARE-logo.svg"
import { Navigate, useNavigate, useOutletContext } from 'react-router';
import { useForm } from 'react-hook-form';
import { useCookies } from 'react-cookie';
import axios from 'axios';
import { useUser } from '../context/UserContext';
import Spinner from '../components/Spinner';
import { useEffect } from 'react';

export default function Login() {


    const { 
        register, 
        handleSubmit, 
        formState: { errors, isSubmitting } 
    } = useForm();
    
    const navigate = useNavigate();
    const { user, login, loading } = useUser();
    const [loginError, setLoginError] = React.useState(null);

    // Redirect if already logged in
    useEffect(() => {
        if (user) {
            navigate('/', { replace: true });
        }
    }, [user, navigate]);

    const onSubmit = async (data) => {
        setLoginError(null);
        try {
            const credentials = {
                Email: data.email,
                Password: data.password
            };
            
            const success = await login(credentials);
            if (!success) {
                setLoginError('Invalid email or password');
            }
        } catch (error) {
            console.error('Login failed:', error);
            const errorMessage = error.response?.data?.message || 
                               error.response?.data?.title || 
                               'Login failed. Please try again.';
            setLoginError(errorMessage);
        }
    };

    if (loading) return <Spinner />;
    if (user) return <Navigate to="/" replace />;

    return (
        <>

            <div style={styles.container}>
                <div style={styles.image}>
                    <div style={styles.ovalWrapper}>
                        <img src={logo} alt="app logo" style={styles.ovalImage} />
                    </div>
                </div>
                <h1 style={styles.title}>Please Log-In</h1>
                <form style={styles.form} onSubmit={handleSubmit(onSubmit)}>
                    <div className="mb-3">
                        <label htmlFor="exampleInputEmail1" style={styles.formLabel}>Email Address</label>
                        <input type="email" className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" {...register('email', { required: true })} />
                        {errors.email && <span>This field is required</span>}
                    </div>
                    <div className="mb-3">
                        <label htmlFor="exampleInputPassword1" style={styles.formLabel}>Password</label>
                        <input type="password" className="form-control" id="exampleInputPassword1" {...register('password', { required: true })} />
                        {errors.password && <span>This field is required</span>}
                    </div>

                    <button type="submit" className="btn btn-primary" backgroundcolor="$004780" style={{ margin: '0 10px' }}>Submit</button>
                </form>
                <p style={styles.registerPrompt}>
                    Haven't created an account? <span onClick={() => navigate('/register')} style={styles.registerLink}>Register here</span>.
                </p>

            </div>
        </>
    );
}

const styles = {
    container: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        minHeight: '100vh',
        width: '100%',
        background: 'linear-gradient(135deg, #004780, #00bfff)',
        padding: '10px',
    },
    title: {
        marginBottom: '20px',
        margin: '20px',
        color: '#fff',
        fontFamily: 'Roboto, sans-serif',
        fontWeight: '600'
    },
    form: {
        width: '80%',
        maxWidth: '400px',
        backgroundColor: '#004780',
        padding: '20px',
        borderRadius: '10px',
        boxShadow: '0 4px 10px rgba(0, 0, 0, 0.2)',
    },
    formLabel: {
      fontWeight: '700',
      fontSize: '1em',
      fontFamily: 'Oxygen',
      color: 'white'

    },
    ovalWrapper: {
        width: '40vw',
        height: '40vh',
        borderRadius: '50%',
        overflow: 'hidden',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        backgroundColor: '#fff', 
        boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)', 
        marginBottom: "5px"
    },
    ovalImage: {
        
        maxWidth: '100%',
        height: 'auto',
        width: 'auto',
    },
    registerPrompt: {
        marginTop: '15px',
        color: '#fff',
        fontSize: '0.9rem',
    },
    registerLink: {
        color: '#ffef00',
        textDecoration: 'underline',
        cursor: 'pointer',
    },
};
