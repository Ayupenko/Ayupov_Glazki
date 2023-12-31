﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ayupov_Glazki
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
   
    public partial class AgentPage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
        public AgentPage()
        {
            InitializeComponent();
            var currentAgent = AyupovGlazkiEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgent;

            ComboSort.SelectedIndex = 0;
            ComboFilter.SelectedIndex = 0;
            EditPriorityBtn.Visibility = Visibility.Hidden;
            UpdateAgents();
        }
        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords=TableList.Count;

            if(CountRecords %10>0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }
            Boolean Ifupdate = true;

            int min;
            if(selectedPage.HasValue) 
            {
                if(selectedPage>=0 && selectedPage<=CountPage) 
                {
                    CurrentPage=(int)selectedPage;
                    min=CurrentPage *10+10<CountRecords ? CurrentPage *10+10 : CountRecords;
                    for(int i=CurrentPage*10;i<min;i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch(direction)
                {
                    case 1:
                        if(CurrentPage>0)
                        {
                            CurrentPage--;
                            min=CurrentPage*10+10<CountRecords ? CurrentPage*10+10 : CountRecords;
                            for(int i=CurrentPage*10; i<min;i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;

                        case 2:
                        if(CurrentPage<CountPage-1)
                        {
                            CurrentPage++;
                            min=CurrentPage*10+10<CountRecords ? CurrentPage *10+10 : CountRecords;
                            for(int i=CurrentPage*10;i<min;i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }    
                        else
                        {
                            Ifupdate=false;
                        }
                        break;
                }
            }
            if(Ifupdate)
            {
                PageListBox.Items.Clear();
                for(int i=1;i<=CountPage;i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex= CurrentPage;
                min=CurrentPage*10+10<CountRecords? CurrentPage*10+10 :CountRecords;
                TBCount.Text=min.ToString();
                TBAllRecords.Text=" из " +CountRecords.ToString();
                AgentListView.ItemsSource = CurrentPageList;
                AgentListView.Items.Refresh();
            }
        }
        private void UpdateAgents()
        {
            var currentAgent = AyupovGlazkiEntities.GetContext().Agent.ToList();
            currentAgent = currentAgent.Where(p => (p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Phone.Replace("+", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Contains(TBoxSearch.Text) || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower()))).ToList();


            if (ComboSort.SelectedIndex == 1)
            {
                currentAgent = currentAgent.OrderBy(p => p.Title).ToList();
            }
            if (ComboSort.SelectedIndex == 2)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();
            }
            if (ComboSort.SelectedIndex == 3)
            {
                currentAgent = currentAgent.OrderBy(p => p.Priority).ToList();
            }
            if (ComboSort.SelectedIndex == 4)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();
            }


            if (ComboFilter.SelectedIndex == 0)
            {
                currentAgent = currentAgent.ToList();
            }
            if (ComboFilter.SelectedIndex == 1)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeID == 1).ToList();
            }

            if (ComboFilter.SelectedIndex == 2)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeID == 2).ToList();
            }

            if (ComboFilter.SelectedIndex == 3)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeID == 3).ToList();
            }
            if (ComboFilter.SelectedIndex == 4)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeID == 4).ToList();
            }
            if (ComboFilter.SelectedIndex == 5)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeID == 5).ToList();
            }
            if (ComboFilter.SelectedIndex == 6)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeID == 6).ToList();
            }
            AgentListView.ItemsSource = currentAgent;
            TableList = currentAgent;
            ChangePage(0,0);

        }
       
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0,Convert.ToInt32(PageListBox.SelectedItem.ToString())-1);
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }
        private void AgentPage_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AyupovGlazkiEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                AgentListView.ItemsSource = AyupovGlazkiEntities.GetContext().Agent.ToList();
            }

            UpdateAgents();
        }
        private void EditPriority_OnClick(object sender, RoutedEventArgs e)
        {
            if (AgentListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите агента/ов для редактирования приоритета");
                return;
            }
            else
            {
                var p = (AgentListView.SelectedItems.Cast<Agent>().Select(selectedItem => selectedItem.Priority)).Prepend(0).Max();
                var window = new EditPriority(p);
                window.ShowDialog();
                if (string.IsNullOrEmpty(window.Priority.Text))
                {
                    return;
                }

                foreach (Agent selectedItem in AgentListView.SelectedItems)
                {
                    selectedItem.Priority = Convert.ToInt32(window.Priority.Text);
                }

                try
                {
                   AyupovGlazkiEntities.GetContext().SaveChanges();
                    window.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                UpdateAgents();
            }
        }
        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditPriorityBtn.Visibility = AgentListView.SelectedItems.Count > 1 ? Visibility.Visible : Visibility.Hidden;
        }
    }
   
    
}
