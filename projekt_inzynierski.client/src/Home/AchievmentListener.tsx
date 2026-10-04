/*import * as signalR from "@microsoft/signalr";
import { useEffect } from "react";

export default function AchievementListener({ userId }) {
  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5086/hubs/achievements")
      .withAutomaticReconnect()
      .build();

    connection.start().then(() => {
      
      connection.invoke("JoinUserGroup", userId);

 
      connection.on("AchievementUnlocked", (data) => {
      
        alert(`Zdoby³eœ osi¹gniêcie: ${data.achievementId}`);
      });
    });

    return () => {
      connection.stop();
    };
  }, [userId]);

  return null;
}
*/