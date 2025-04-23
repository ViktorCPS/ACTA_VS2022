var myclose = false;

// add try catch if use any function!!!!

function ConfirmClose()
{
	if (event.clientY < 0 && event.clientX >= screen.width)
	{
		event.returnValue = 'Any message you want';

		setTimeout('myclose=false',100);
		myclose=true;
	}
}

function HandleOnClose()
{
	if (myclose==true) alert("Window is closed");
}

function Logout() 
{
	if (event.clientY < 0) 
	{
		//Bilja, 08.12.2006
		//potrebno da ode na ovu stranu da bi se izazvao session_end
		document.location.href="/ACTAWeb/Logout.aspx";
	}
}

function LogoutByButton() 
{
    if (confirm("Are you sure you want to log out?") == true)
    {
        //Bilja, 08.12.2006
	    //potrebno da ode na ovu stranu da bi se izazvao session_end
	    my_window = window.open('/ACTAWeb/Logout.aspx','logout','width=10,height=10,resizable=no,position=absolute,left=280,top=240');
	    //ne registruje bez ovog timeout-a
	    setTimeout('',10);
    	
	    //ovaj if sluzi da se ne bi javio onaj Dialog box na close ("Are you sure...")
	    if(!window.opener){
		    window.opener = '';
	    }
	    window.close();	//zatvara se strana
	    my_window.close();
    }	
    
    return true;
}

/* Kod help ekrana se Brauzerska kontrola (pa i Exit dugme) nalazi u IFrame */
function LogoutByButtonHelpScreens() 
{
	if (confirm("Are you sure you want to log out?") == true)
	{
	  //Bilja, 08.12.2006
      //potrebno da ode na ovu stranu da bi se izazvao session_end
	  my_window = window.open('/ACTAWeb/Logout.aspx','logout','width=10,height=10,resizable=no,position=absolute,left=280,top=240');
	  //ne registruje bez ovog timeout-a
	  setTimeout('',10);
	  
	  if (parent.opener.opener)
	  {
	    //ovaj if sluzi da se ne bi javio onaj Dialog box na close ("Are you shure...")
		if(!parent.opener.opener.window.opener){
	      parent.opener.opener.window.opener = '';
		}
		parent.opener.opener.window.close(); //na Result za Track by Ref je slucaj sa 3 prozora: Result, Track by Ref Entry (novi prozor) i Help, pa mora i taj treci da se zatvori
	  }
	
	  //ovaj if sluzi da se ne bi javio onaj Dialog box na close ("Are you shure...")
	  if(!parent.opener.window.opener){
	    parent.opener.window.opener = '';
	  }
	  parent.opener.window.close(); //zatvara se strana na kojoj je help, sto je ili neka Entry strana ili Tracking
	  
	  //ovaj if sluzi da se ne bi javio onaj Dialog box na close ("Are you shure...")
	  if(!parent.window.opener){
		parent.window.opener = '';
	  }
	  parent.window.close();  //zatvara se help strana
	  
	  my_window.close();
	}
	return true;
}