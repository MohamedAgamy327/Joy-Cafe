   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5       Cafe3   -Cafe.Views.ClientViews.ClientPointUserControl   5      HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5      R NMahApps.Metro.IconPacks, Version=1.9.1.0, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5      n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      b ^GalaSoft.MvvmLight.Platform, Version=5.4.1.0, Culture=neutral, PublicKeyToken=5f873c45e98af8a1-Custom http://www.galasoft.ch/mvvmlight  5      R	 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml    	 5	      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006    	 5
      @d2http://schemas.microsoft.com/expression/blend/2008    	 5        >�Language$  ar-EG�5      $��RightToLeft=�.  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *Loaded+5       � 	Resources 5       �� MergedDictionaries �� Source:  4System.Windows.Controls.Primitives.DatePickerTextBox �� 
TargetType	 �� Property
 �� Value ��Name �� Path �� StringFormat �� RelativeSource �� AncestorType(  "System.Windows.Controls.DatePicker�� 5   
    5      �� 5      $ ..\..\Skins\MainSkin.xaml?�6%   %�  '        '  �     �� 5      "  6   .�5       �� 	 � ��
 5!      �� 5"      .R�5#      �� $ PART_TextBox��6&   ��5$      �� $ SelectedDate�$ 
yyyy/MM/dd�� �� "  5&      5(      5)      �� 5*      "  6   .�5+      �� 	 � ��$
 calibri)�6/   �� 5,      	 � ��$

 Bold"�6/   �� 5-      	 � ��$
 16(�6-   �� 5.      	 � ��$
 30��6+   5/      51      .��54      � $��1��6    � RowDefinitions 55   
   �� 56      $��1* �6   �� 57      $��8* �6   �� 58      $��1* �6   �� 59      $	��.8* �6   .+�5;   
   � $
��0,10q�6    � ColumnDefinitions 5<      �� 5=      $��3* �6#   �� 5>      $
��1.5* �6#   .+�5A      �� $	��5,0q�6   $��0��6$   *   $MahApps.Metro.Controls.TextBoxHelper   ClearTextButton .    UseFloatingWatermark .    	Watermark$ 	بحث...��5D      $��14(�5E      $��calibri)�6   $
��Bold"�64   ��5A   4   �� *Key+ �� UpdateSourceTrigger$ PropertyChanged=�.��5F      ��5G      �� 5H         SearchMetroTextBox �� BasedOn# [  6   " ��6J    �� Triggers 5I      P� 5J       P� Property �@ HasText P� Value . . �5K   "   �� 	 �@ ButtonContent$
 r��6Y   �� 5L   "   	 �@ ButtonContentTemplate
 6a   Z� 5M      5O      .��5P   !    J� InputBindings 5Q      �� 5R      
 �� Key$ Enter��6A    �� Command 6!   �� $ Search�6A   .��5S   )   5T      �� 5U       �� Orientation$ 
Horizontal=�6   $��1��62   .+�5V      �� $	��5,0q�6     TextBlockAddUpdateStyle#��[ 6)   من6_     5X      $	��5,0q�6f      SelectedDate 6   �� *
DateFrom+$ PropertyChanged=� 5Y        5Z      $ SelectedDateChanged��6)   . 5[      /  )GalaSoft.MvvmLight.Command.EventToCommand     Command 64   �� *Search+5\      5^      �� 5_        TextBlockStyle#��[ 6   $��Center=�6D   $	��5,0q�6_   إلى6q     5`      $	��5,0q�6e    6!   �� *DateTo+$ PropertyChanged=� 5a        5b      $ SelectedDateChanged��6)   . 5c         64   �� *Search+5d      5f      5g      5h      &	   System.Windows.Controls.DataGrid	  5i   
   $��0 10 0 0q�5k      $��1��5l        	  HeadersVisibility$	  All=�5m      ! 	  SelectionUnit$! FullRow=�5n      " J� IsManipulationEnabled" .  # 	  CanUserDeleteRows# .  $ 	  CanUserAddRows$ .  % 	  AutoGenerateColumns% .  ��5j      �� *	Clients+& 	  Columns& 5t      0
  *System.Windows.Controls.DataGridTextColumn
  5u      ,  &System.Windows.Controls.DataGridColumn'   
IsReadOnly' . (   Header$( العميل��68   )   Width5  /System.Windows.Controls.DataGridLengthConverter$) 2* 6H     ColumnHeaderStyle2*   HeaderStyle#* [ 6T   1  +System.Windows.Controls.DataGridBoundColumn+   Binding+ 6�   �� *Client.Name+
  5v      ' . $( 
الكود��68   $) 1* 6G   #* [ 6S   + 6�   �� *Client.Code+
  5w      ' . $( التليفون��68   $) 1* 6J   #* [ 6V   + 6�   �� *Client.Telephone+
  5x      ' . $( النقاط��68   $) 1* 6H   #* [ 6T   + 6�   �� *Points+��5{      �� 5|        MetroDataGrid# [ 6   " 	 5}      .�5~      ��  	 �@	 AlternatingRowBackground  GrayBrush10#
 �  5      5�      5�      �� 5�   
   $	��0,5q�6   $��2��6#   $ 
Horizontal=�60   $��Center=�6I   $��Bottom=�6f   .+�5�      �� $	��5,0q�6U     MetroCircleButtonStyle#���  6e   $��0q�6�   ��6   �� *
Previous+{�62   �� *Paging.First+.��5�      .  (MahApps.Metro.IconPacks.PackIconOcticons  �  �ControlzEx.PackIconBase`1[[MahApps.Metro.IconPacks.PackIconOcticonsKind, MahApps.Metro.IconPacks, Version=1.9.1.0, Culture=neutral, PublicKeyToken=null]],   Kind$, ChevronRight=�6-   $��20��6A   $��20��6L   5�      �� 5�      $	��5,0q�6<   $��Center=�6I   #��[ 6d   ��6   �� *Paging.CurrentPage+6d   �� 5�      $
��من��6   $	��5,0q�6"   $��Center=�6/   #��[ 6J   �� 5�      $	��5,0q�69   $��Center=�6F   #��[ 6a   ��6   �� *Paging.LastPage+6a   �� 5�      $	��5,0q�6Q   #���  6`   $��0q�6�   ��6   �� *Next+{�6.   �� *Paging.Last+.��5�        $, ChevronLeft=�6-   $��20��6@   $��20��6K   5�      5�      �� 5�   
   $��Bottom=�6   $��3��6/   .@�5�      �� $��عدد العملاء��6     StatusBarItemStyle#��[ 62   �� 5�        MetroStatusBarSeparator#��[ 6   6K   �� 5�      #��[ 6E   ��6   �� *Paging.TotalRecords+6E   5�      5�      5�      