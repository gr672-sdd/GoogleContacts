   M S B A M L   `   `   `  ����L  HSorokinDotNetTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:SorokinDotNetTestSorokinDotNetTest  5   	   "    SorokinDotNetTest.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:SorokinDotNetTest     5   	     � Title$  Авторизация��5   	   $	��800��6   $	��700��6*   $��Ebrima)�66    � WindowStartupLocation$ CenterScreen=�6J   $��18(�6o   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��0.35* �6   �� 5      $��0.05* �6   �� 5      �� 5      $��0.35* �6    � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $
��0.8* �6   �� 5      $
��0.3* �6   �� 5      $
��0.8* �6   �� 5      $
��0.2* �6   �� 5      $
��0.6* �6   �� 5      $
��0.4* �6   .+�5   
   �� $0��*pack://application:,,,/Resources/Frost.jpg��6   $��8��6D   $��10��6X   $
��Fill=�6j   �� 5   
   -   5         ��Name$ 
tbIdClient��5      $��0��6)   $��1��69   $ ��Идентификатор��6F    �� TextAlignment$ Right=�6[   
�������6q   �� 5   
   -   5        $ SecretClient��5      $��Ключ��6+   $��0��67   $��3��6G   $ Right=�6T   
�������6j   �� 5    
   -   5         ��Name$ tboxIdClient��5       $��1��6)   $��2��67    �� 	MaxLength$ 80��6G    �� TextWrapping$
 Wrap=�6�   
������ 
���������6V   �� *
IdClient+	 �� UpdateSourceTrigger$	 PropertyChanged=�6�   �� 5"   
   -   5        $ tboxSecretClient��5"      $��3��6,   $��2��69   $
 Wrap=�6�   
������ 
���������6I   �� *SecretClient+$	 PropertyChanged=�6�   �� 5$   
   -   5        
 ��Name$
 butAutorization��5$      $��5��6*   $��2��67   $��Авторизация��6G   
������ 
���������6]   �� *AuthorizeCommand+6�   5%      5&      