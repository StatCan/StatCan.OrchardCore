export default function IsDarkMode()
{
  let darkModeEnabled = false;

  if (document.documentElement.dataset.displayMode == 'picker') 
  {
    darkModeEnabled = localStorage.getItem("VuetifyThemeDarkMode") === 'true';
  } 
  else if (document.documentElement.dataset.displayMode == 'dark') 
  {
    darkModeEnabled = true;
  }

  return darkModeEnabled;
}
