/* eslint-disable @typescript-eslint/no-unused-vars */

import LoginFormComponent from "./LoginFormComponent";
import "./NiceButton.css"
import { useState } from 'react';

import RegisterWizard from './Register/RegisterWizard';
import { TextHoverEffect } from '../components/ui/shadcn-io/text-hover-effect';
function LoginPanelComponent() {

    const [logOrNot, setLog] = useState<boolean>(true)
    return (
        <div className="flex flex-col items-center justify-center gap-2" >

                <TextHoverEffect text="CodeOdyssey" duration={0.1 }  />
       

            
          
            {logOrNot ? (
                <LoginFormComponent key="login" />
            ) : (
                <RegisterWizard key="register" />
            )}


            <button onClick={() => setLog(!logOrNot)} className=" text-indigo-600 mt-4  hover:text-white hover:bg-indigo-800 hover:ease-in cursor-pointer rounded-2xl border bg-white p-2">
                {logOrNot ? "A może chcesz założyć konto?" : "Mam już konto!"}
                
                </button>
            </div>
        
    );
}

export default LoginPanelComponent;
