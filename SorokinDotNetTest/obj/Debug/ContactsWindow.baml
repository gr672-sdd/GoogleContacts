   M S B A M L   `   `   `  ����L  HSorokinDotNetTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:SorokinDotNetTestSorokinDotNetTest  5   	   &     SorokinDotNetTest.ContactsWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:SorokinDotNetTest     5   	     � Title$  Контакты��5   	   $	��800��6   $
��1350��6'   $��Ebrima)�64   $��18(�6H    � WindowStartupLocation$ CenterScreen=�6V    � DataContext 5	   	   �� *ContactsWindowViewModel+   locator �� Source# [  .��5
      �  � RowDefinitions 5   
   �� 5      $
��0.2* �6   �� 5      $
��0.5* �6   �� 5      $
��0.6* �6   �� 5      $
��0.3* �6   �� 5      $
��0.6* �6   �� 5      $��3* �6   �� 5      $��3* �6   �� 5      $
��0.6* �6   �� 5      $
��0.4* �6    � ColumnDefinitions 5   
   �� 5      $��0.05* �6   �� 5      $��0.48* �6   �� 5      $
��0.1* �6   �� 5      �� 5      �� 5      �� 5      .+�5   
   �� $0��*pack://application:,,,/Resources/Frost.jpg��6   $��8��6D   $��10��6X   $
��Fill=�6j   �� 5"   
   $��3��5#      $��4��6#   $��4��67   $��4��6D   
������ ��5"      �� *
PeopleList+ �� SelectedItem 66   �� *SelectedContact+��5$      �� 5%      .R�5&      �� .+�5'      �� 
������ 
���������6!   �� * DataContext.EditContactCommand+ �� RelativeSource ��  �� AncestorType$ ListViewL���5(   !   �� .��5)      �� .+�5*   "   �  5+   &   �� 5,   *   $	��100 �6;   �� 5-   *   $	��350 �6;   �� 5.   *   $	��200 �6;   �� 5/   *   $	��150 �6;   �� 50   *   $	��250��6;   .+�52   &   �� $��50��6M   $��50��6X   $��0��6d   ��6,   �� *Photos[0].Url+6d   �� 53   &   $��1��61   $��Center=�6A   .+�54   *   �  55   .   �� 56   2   $
��auto �6C   �� 57   2   $
��auto �6C   .+�59   .   �� $��0��6\   $��10,0,0,0q�6l   ��68   �� *Names[0].GivenName+6l   �� 5:   .   $��1��6]   $��10,0,0,0q�6m   ��68   �� *Names[0].FamilyName+6m   5;   +   5<   '   �� 5=   &   $��2��6Y   $��10,0,0,0q�6i   $��Center=�6{   ��60   �� *EmailAddresses[0].Value+6{   �� 5>   &   $��3��6W   $��10,0,0,0q�6g   $��Center=�6y   ��60   �� *PhoneNumbers[0].Value+6y   �� 5?   &   $��4��61   $��Center=�6A   .+�5@   *   �  5A   .   �� 5B   2   $
��auto �6C   �� 5C   2   $
��auto �6C   .+�5E   .   �� $��0��6_   $��10,0,0,0q�6o   ��68   �� *Organizations[0].Name+6o   �� 5F   .   $��1��6`   $��10,0,0,0q�6p   ��68   �� *Organizations[0].Title+6p   5G   +   5H   '   5I   #   5J      5K      5L      5M      5O      �� 5R   
   -   5        	 ��Name$	 butCreateContact��5R      $��Новый контакт��6+   $��4��5T      $��1��6   
������ 
���������5S      �� *CreateContactCommand+5T   G   �� 5W   
   -   5        $	 butClose��5W      $��
Назад��6#   $��7��5Y      $��1��6   
������ 
���������5X      �� *CloseCommand+5Y   G   �� 5[   
   $2��,pack://application:,,,/Resources/seartch.png��6   $��2��6F   $��1��6V   �� 5^   
   -   5        
 ��Name$
 tbSearch��5^      $��1��5_      $��3��6   ��5^   $   �� *
SearchText+ �� UpdateSourceTrigger$ PropertyChanged=�.��5_   /   ��5`      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5a   -   �� 6   6sys&clr-namespace:System;assembly=mscorlib     6-    �� 
TargetType" ��6    �� 	Resources 5b        CueBannerBrush j� 
AlignmentX j� 
AlignmentY j� Stretch !� Visual��%�   &	       !� 5c      $
 Left=�6=   $ Center=�6O   $
 None=�6c    5d      �� 5e   "   $��
Поиск��6(   
�������$��20(�6O   5g      5h       �� Triggers 5i      P� 5j       P� Property � �� �� Empty P� Value# Z 62   . �5k      ��  �� Property � �� �� Value# [ 6;   5l      P� 5m       � �� 62   Z� . �5n      ��  � ��# [ 6;   5o      P� 5p       �@J�IsKeyboardFocused . . �5q      ��  � ��
 �����6;   5r      5t      .��5u      5v      5w      5x      