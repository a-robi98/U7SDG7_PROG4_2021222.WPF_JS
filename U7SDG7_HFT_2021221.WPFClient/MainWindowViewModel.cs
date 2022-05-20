using Esport.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace U7SDG7_HFT_2021221.WPFClient
{
    class MainWindowViewModel : ObservableRecipient
    {
        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        // Locations

        public RestCollection<Location> Locations { get; set; }

        private Location selectedLocation;

        public Location SelectedLocation
        {
            get { return selectedLocation; }
            set {
                if (value != null)
                {
                    selectedLocation = new Location()
                    {
                        ID = value.ID,
                        Name = value.Name,
                        Capacity = value.Capacity
                    };
                    OnPropertyChanged();
                    (DeleteLocationCommand as RelayCommand).NotifyCanExecuteChanged();
                    //(UpdateLocationCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }
        }

        public ICommand CreateLocationCommand { get; set; }
        public ICommand DeleteLocationCommand { get; set; }
        public ICommand UpdateLocationCommand { get; set; }

        // Matches 

        public RestCollection<Match> Matches { get; set; }

        public ICommand CreateMatchCommand { get; set; }
        public ICommand UpdateMatchCommand { get; set; }
        public ICommand DeleteMatchCommand { get; set; }

        private Match selectedMatch;

        public Match SelectedMatch
        {
            get { return selectedMatch; }
            set
            {
                if (value != null)
                {
                    selectedMatch = new Match()
                    {
                        Name = value.Name,
                        ID = value.ID,
                        Location = value.Location,
                        Team1ID = value.Team1ID,
                        Team2ID = value.Team2ID,
                    };
                    OnPropertyChanged();
                    (DeleteMatchCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }
        }

        // Teams

        public RestCollection<Team> Teams { get; set; }

        public ICommand CreateTeamCommand { get; set; }
        public ICommand UpdateTeamCommand { get; set; }
        public ICommand DeleteTeamCommand { get; set; }

        private Team selectedTeam;

        public Team SelectedTeam
        {
            get { return selectedTeam; }
            set
            {
                if (value != null)
                {
                    selectedTeam = new Team()
                    {
                        Name = value.Name,
                        Odd = value.Odd,
                        BettedAmount = value.BettedAmount,
                        ID = value.ID,
                        //Match = value.Match,
                        MatchID = value.MatchID,
                        Wins = value.Wins,
                    };
                    OnPropertyChanged();
                    (DeleteTeamCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }
        }

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }

        public MainWindowViewModel()
        {
            if (!IsInDesignMode)
            {
                // Locations

                Locations = new RestCollection<Location>("http://localhost:22389/", "location", "hub");

                CreateLocationCommand = new RelayCommand(() =>
                {
                    Locations.Add(new Location()
                    {
                        Name = SelectedLocation.Name,
                        Capacity = SelectedLocation.Capacity
                    });
                });

                UpdateLocationCommand = new RelayCommand(() =>
                {

                    try
                    {
                        Locations.Update(SelectedLocation);
                    }
                    catch (ArgumentException ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                },
                () =>
                {
                    return SelectedLocation != null;
                });

                DeleteLocationCommand = new RelayCommand(() =>
                {
                    Locations.Delete(SelectedLocation.ID);
                },
                () =>
                {
                    return SelectedLocation != null;
                });
                SelectedLocation = new Location();

                // Matches

                Matches = new RestCollection<Match>("http://localhost:22389/", "match", "hub");

                CreateMatchCommand = new RelayCommand(() =>
                {
                    Matches.Add(new Match()
                    {
                        Name = SelectedMatch.Name,
                        //ID = SelectedMatch.ID,
                        Location = SelectedMatch.Location,
                        Team1ID = SelectedMatch.Team1ID,
                        Team2ID = SelectedMatch.Team2ID
                    });
                });

                DeleteMatchCommand = new RelayCommand(() =>
                {
                    Matches.Delete(SelectedMatch.ID);
                },
                () =>
                {
                    return SelectedMatch != null;
                });

                UpdateMatchCommand = new RelayCommand(() =>
                {
                    try
                    {
                        Matches.Update(SelectedMatch);
                    }
                    catch (ArgumentException ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                });
                SelectedMatch = new Match();

                //Teams

                Teams = new RestCollection<Team>("http://localhost:22389/", "team", "hub");


                CreateTeamCommand = new RelayCommand(() =>
                {
                    Teams.Add(new Team()
                    {
                        Name = SelectedTeam.Name,
                        //ID = SelectedTeam.ID,
                        Wins = SelectedTeam.Wins,
                        BettedAmount = SelectedTeam.BettedAmount,
                        Odd = SelectedTeam.Odd
                    });
                });
                UpdateTeamCommand = new RelayCommand(() =>
                {
                    try
                    {
                        Teams.Update(SelectedTeam);
                    }
                    catch (ArgumentException ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                });
                DeleteTeamCommand = new RelayCommand(() =>
                {
                    Teams.Delete(SelectedTeam.ID);
                },
                () =>
                {
                    return SelectedTeam != null;
                });
                SelectedTeam = new Team();

            }


        }
    }
}
