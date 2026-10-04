/* eslint-disable @typescript-eslint/no-unused-vars */
import React, {  type ReactNode } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';

import LoginPanelComponent from './Auth/LoginPanelComponent';
import DashboardComponent from './Home/DashboardComponent';
import UserCoursesComponent from './CoursesNavigation/UserCoursesComponent';
import PopularCoursesComponent from './CoursesNavigation/PopularCoursesComponent';
import LatestCoursesComponent from './CoursesNavigation/LatestCoursesComponent';
import ChallengesComponent from './Challenges/ChallengesComponent';
import AssistantComponent from './Assistant/AssistantComponent';
import UserAchievmentsComponent from './Achievments/UserAchievmentsComponent';
import FaqComponent from './FAQ/FaqComponent';

import MainCoursePageComponent from './Courses/MainCoursePageComponent';

import SubjectPageComponent from './Courses/SubjectPageComponent';
import TheoryComponent from './Courses/TheoryComponent';
import ExerciseSolutionComponent from './Courses/ExerciseSolutionComponent';
import AdaptiveCourseCreator from './adaptive-courses/AdaptiveCourseCreator';
import Notifications from './Navigation/Notifications';
import RewardShop from './PremiumStore/RewardShop';
import UserProfile from './Profile/UserProfile';
import CodeOdysseyHome from './Home/CodeOdysseyHome';
import NavBarComponent from './Navigation/NavBarComponent';
import AdminPanel from './Admin/AdminPanel';


import { useAuth } from './Auth/AuthContext';
import CodeRunner from './Playground/CodeRunner';
import PVPComponent from './PVP/PVPComponent';
import PVPGameComponent from './PVP/PVPGameComponent';

interface PrivateRouteProps {
    element: ReactNode;
    requireAdmin?: boolean;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ element, requireAdmin = false }) => {
    const { isLogged, userRole, loading } = useAuth();

    if (loading) {
        return <div>Ładowanie...</div>;
    }

    if (!isLogged) {
        return <Navigate to="/login" />;
    }
    console.log("Rola" + userRole)

    if (userRole === "admin") {

        if (requireAdmin) {
            return <>{element}</>;
        }

        else {
            return <Navigate to="/admin" />;
        }
    }



    if (requireAdmin && userRole !== "admin") {
        return <Navigate to="/dashboard" />;
    }


    return <>{element}</>;
};

const AppContent = () => {
    return (
        <>
            {/* NavBar wewnątrz Router */}
            <div className="sticky top-0 z-2137 bg-white/80 backdrop-blur-sm">
                <NavBarComponent />
            </div>

            <div className="mx-12 mb-14">
                <Routes>
                    <Route path="/login" element={<LoginPanelComponent />} />
                    <Route path="/faq" element={<FaqComponent />} />
                    <Route path="/" element={<CodeOdysseyHome />} />
                    <Route path="/dashboard" element={<PrivateRoute element={<DashboardComponent />} />} />
                    <Route path="/challenges" element={<PrivateRoute element={<ChallengesComponent />} />} />
                    <Route path="/latestCourses" element={<PrivateRoute element={<LatestCoursesComponent />} />} />
                    <Route path="/myCourses" element={<PrivateRoute element={<UserCoursesComponent />} />} />
                    <Route path="/popularCourses" element={<PrivateRoute element={<PopularCoursesComponent />} />} />
                    <Route path="/assistant" element={<PrivateRoute element={<AssistantComponent />} />} />
                    <Route path="/exercise/:exerciseId" element={<ExerciseSolutionComponent />} />
                    <Route path="/course/:courseId" element={<MainCoursePageComponent />} />
                    <Route path="/course/:courseId/subject/:subjectId" element={<SubjectPageComponent />} />
                    <Route path="/theory/:subjectId" element={<TheoryComponent />} />
                    <Route path="/userAchievments" element={<PrivateRoute element={<UserAchievmentsComponent />} />} />
                    <Route path="/adaptive" element={<PrivateRoute element={<AdaptiveCourseCreator />} />} />
                    <Route path="/notifications" element={<PrivateRoute element={<Notifications />} />} />
                    <Route path="/store" element={<PrivateRoute element={<RewardShop />} />} />
                    <Route path="/profile" element={<PrivateRoute element={<UserProfile />} />} />
                    <Route path="/playground" element={<PrivateRoute element={<CodeRunner />} />} />
                    <Route path="/pvp" element={<PrivateRoute element={<PVPComponent />} />} />
                    <Route path="/pvp/game/:sessionId" element={<PrivateRoute element={<PVPGameComponent />} />} />

                    <Route path="/admin" element={<PrivateRoute element={<AdminPanel />} requireAdmin={true} />} />
                </Routes>
            </div>
        </>
    );
}

const App = () => {
    return (
        <Router>
            <AppContent />
        </Router>
    );
};

export default App;