   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5       CafeB   <Cafe.Views.MembershipViews.ClientMembershipMinuteUserControl   5      HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5      R NMahApps.Metro.IconPacks, Version=1.9.1.0, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5      n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5	      @d2http://schemas.microsoft.com/expression/blend/2008     5
        >�Language$  ar-EG�5      $��RightToLeft=�.  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *Loaded+5       � 	Resources 5       �� MergedDictionaries �� Source:  4System.Windows.Controls.Primitives.DatePickerTextBox �� 
TargetType	 �� Property
 �� Value ��Name �� Path �� StringFormat �� RelativeSource �� AncestorType(  "System.Windows.Controls.DatePicker�� 5   
    5      �� 5      $ ..\..\Skins\MainSkin.xaml?�6%   %�  '        '  �     �� 5      "  6   .�5      �� 	 � ��
 5      �� 5      .R�5      �� $ PART_TextBox��6&   ��5       �� $ SelectedDate�$ 
yyyy/MM/dd�� �� "  5"      5$      5%      �� 5&      "  6   .�5'      �� 	 � ��$
 calibri)�6/   �� 5(      	 � ��$

 Bold"�6/   �� 5)      	 � ��$
 16(�6-   �� 5*      	 � ��$
 30��6+   5+      5,      .��5/      � $��1��6    � RowDefinitions 50   
   �� 51      $��1* �6   �� 52      $
��8.5* �6   �� 53      $��1* �6   �� 54      $
��0.8* �6   .+�56   
   � $
��0,10q�6   .+�58      �� $	��5,0q�6   $��0��6$   *   $MahApps.Metro.Controls.TextBoxHelper   ClearTextButton .    UseFloatingWatermark .    	Watermark$ 	بحث...��5;      $��14(�5<      $��calibri)�6   $
��Bold"�64   ��58   4   �� *Key+ �� UpdateSourceTrigger$ PropertyChanged=�.��5=      ��5>      �� 5?         SearchMetroTextBox �� BasedOn# [  6   " ��6J    �� Triggers 5@      P� 5A       P� Property �@ HasText P� Value . . �5B   "   �� 	 �@ ButtonContent$
 r��6Y   �� 5C   "   	 �@ ButtonContentTemplate
 6a   Z� 5D      5F      .��5G   !    J� InputBindings 5H      �� 5I      
 �� Key$ Enter��6A    �� Command 6!   �� $ Search�6A   .��5J   )   5K      5L      &   System.Windows.Controls.DataGrid  5M   
   $��0 10 0 0q�5O      $��1��5P         HeadersVisibility$	 All=�5Q         SelectionUnit$ FullRow=�5R       J� IsManipulationEnabled .     CanUserDeleteRows .      CanUserAddRows  .  !   AutoGenerateColumns! .  ��5N      �� *ClientMembershipMinutes+"   Columns" 5X      0	  *System.Windows.Controls.DataGridTextColumn	  5Y      ,
  &System.Windows.Controls.DataGridColumn# 
  
IsReadOnly# . $ 
  Header$$ العميل��68   % 
  Width5  /System.Windows.Controls.DataGridLengthConverter$% 2* 6H     ColumnHeaderStyle4& 
  HeaderStyle#& [ 6T   1  +System.Windows.Controls.DataGridBoundColumn'   Binding' 6�   �� *Client.Name+	  5Z      # . $$ 
الكود��68   $% 1* 6G   #& [ 6S   ' 6�   �� *Client.Code+	  5[      # . $$ التليفون��68   $% 1* 6J   #& [ 6V   ' 6�   �� *Client.Telephone+	  5\      # . $$ 
النوع��68   $% 1* 6G   #& [ 6S   ' 6�   �� *DeviceType.Name+	  5]      # . $$ الدقائق��68   $% 1* 6I   #& [ 6U   ' 6�   �� * ClientMembershipMinute.Minutes+��5`      �� 5a        MetroDataGrid# [ 6   "  5b      .�5c      ��  	 �@ AlternatingRowBackground  GrayBrush10#
 �  5d      5e      5h      �� 5j   
   $	��0,5q�6   $��2��6#   ( �� Orientation$( 
Horizontal=�60   $��Center=�6I   $��Bottom=�6f   .+�5k      �� $	��5,0q�6U     MetroCircleButtonStyle#���  6e   $��0q�6�   ��6   �� *
Previous+{�62   �� *Paging.First+.��5l      .  (MahApps.Metro.IconPacks.PackIconOcticons  �  �ControlzEx.PackIconBase`1[[MahApps.Metro.IconPacks.PackIconOcticonsKind, MahApps.Metro.IconPacks, Version=1.9.1.0, Culture=neutral, PublicKeyToken=null]])   Kind$) ChevronRight=�6-   $��20��6A   $��20��6L   5m      �� 5n      $	��5,0q�6<   $��Center=�6I     TextBlockStyle#��[ 6d   ��6   �� *Paging.CurrentPage+6d   �� 5o      $
��من��6   $	��5,0q�6"   $��Center=�6/   #��[ 6J   �� 5p      $	��5,0q�69   $��Center=�6F   #��[ 6a   ��6   �� *Paging.LastPage+6a   �� 5q      $	��5,0q�6Q   #���  6`   $��0q�6�   ��6   �� *Next+{�6.   �� *Paging.Last+.��5r        $) ChevronLeft=�6-   $��20��6@   $��20��6K   5s      5t      �� 5v   
   $��Bottom=�6   $��3��6/   .@�5w      �� $!��عدد الاشتراكات��6     StatusBarItemStyle#��[ 65   �� 5x        MetroStatusBarSeparator#��[ 6   6K   �� 5y      #��[ 6E   ��6   �� *Paging.TotalRecords+6E   5z      5|      5�      