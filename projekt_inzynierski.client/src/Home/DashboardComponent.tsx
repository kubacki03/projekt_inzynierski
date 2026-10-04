
import FeaturedCoursesComponent from "./FeaturedCoursesComponent";
import UserProgressComponent from "./UserProgressComponent";
import UserLastAchievmentComponent from "./UserLastAchievmentComponent";
import LeaderBoardComponent from "./LeaderBoardComponent";
import "./stars.css"
import TypingAnimation from "./TypingAnimation";



function DashboardComponent() {

    
  return (
      <div className="mt-2 min-h-screen text-white">
        

          <h1 className="focus-in-expand-fwd font-mono text-6xl italic">2025: A CODE ODYSSEY</h1>
          <TypingAnimation />

          <hr className="bg-white"></hr>
          <div className="mx-[1%] mt-[3%]">
              <FeaturedCoursesComponent />
              

           

              <div className="mt-[10%] flex justify-center">
                  <div className="flex w-full max-w-[1200px] flex-col justify-between md:flex-row">
                      <UserProgressComponent />
                      <UserLastAchievmentComponent />
                      <LeaderBoardComponent />
                  </div>

              </div>


            

          </div>
          
      </div>
  );
}

export default DashboardComponent;