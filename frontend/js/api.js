// ===== ProfRate API Service =====

const API_URL = '/api';

// Helper function للـ API calls
async function apiCall(endpoint, method = 'GET', data = null) {
    const options = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        }
    };

    // إضافة الـ Token لو موجود
    const token = localStorage.getItem('token');
    if (token) {
        options.headers['Authorization'] = `Bearer ${token}`;
    }

    // إضافة الـ Body لو فيه Data
    if (data) {
        options.body = JSON.stringify(data);
    }

    try {
        const response = await fetch(`${API_URL}${endpoint}`, options);
        const result = await response.json();
        return { success: response.ok, data: result };
    } catch (error) {
        console.error('API Error:', error);
        return { success: false, data: { message: 'خطأ في الاتصال بالسيرفر' } };
    }
}

// ===== Auth API =====
const AuthAPI = {
    login: (username, password, userType) => {
        return apiCall('/auth/login', 'POST', { username, password, userType });
    }
};

// ===== Students API =====
const StudentsAPI = {
    getAll: () => apiCall('/students/GetAll'),
    getById: (id) => apiCall(`/students/GetById/${id}`),
    add: (data) => apiCall('/students/Add', 'POST', data),
    update: (id, data) => apiCall(`/students/Update/${id}`, 'PUT', data),
    delete: (id) => apiCall(`/students/Delete/${id}`, 'DELETE')
};

// ===== Lecturers API =====
const LecturersAPI = {
    getAll: () => apiCall('/lecturers/GetAll'),
    getById: (id) => apiCall(`/lecturers/GetById/${id}`),
    add: (data) => apiCall('/lecturers/Add', 'POST', data),
    update: (id, data) => apiCall(`/lecturers/Update/${id}`, 'PUT', data),
    delete: (id) => apiCall(`/lecturers/Delete/${id}`, 'DELETE')
};

// ===== Questions API =====
const QuestionsAPI = {
    getAll: () => apiCall('/questions/GetAll'),
    add: (data) => apiCall('/questions/Add', 'POST', data),
    update: (id, data) => apiCall(`/questions/Update/${id}`, 'PUT', data),
    delete: (id) => apiCall(`/questions/Delete/${id}`, 'DELETE')
};

// ===== Subjects API =====
const SubjectsAPI = {
    getAll: () => apiCall('/subjects/GetAll'),
    add: (data) => apiCall('/subjects/Add', 'POST', data),
    delete: (id) => apiCall(`/subjects/Delete/${id}`, 'DELETE')
};

// ===== Evaluations API =====
const EvaluationsAPI = {
    getAll: () => apiCall('/evaluations/GetAll'),
    getReport: () => apiCall('/evaluations/GetReport'),
    add: (data) => apiCall('/evaluations/Add', 'POST', data)
};

// ===== Helper Functions =====

// عرض Alert
function showAlert(message, type = 'success') {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type}`;
    alertDiv.textContent = message;

    const container = document.querySelector('.main-content') || document.body;
    container.insertBefore(alertDiv, container.firstChild);

    setTimeout(() => alertDiv.remove(), 3000);
}

// التحقق من تسجيل الدخول
function checkAuth() {
    const token = localStorage.getItem('token');
    const userType = localStorage.getItem('userType');

    if (!token) {
        window.location.href = '../login.html';
        return false;
    }
    return { token, userType };
}

// تسجيل الخروج
function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userType');
    localStorage.removeItem('userId');
    window.location.href = '../login.html';
}
