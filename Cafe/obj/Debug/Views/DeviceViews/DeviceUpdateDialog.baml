   M S B A M L   `   `   `  ����?  ;Cafe, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null+clr-namespace:Cafe.HelpersCafe.Helpers  5      U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5      fCclr-namespace:MahApps.Metro.Controls.Dialogs;assembly=MahApps.MetroMahApps.Metro.Controls.Dialogs 5      /    )Cafe.Views.DeviceViews.DeviceUpdateDialog   5      &Helpersclr-namespace:Cafe.Helpers  5      HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5      n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      WdialogsCclr-namespace:MahApps.Metro.Controls.Dialogs;assembly=MahApps.Metro     5      R NMahApps.Metro.IconPacks, Version=1.9.1.0, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5	      4  .MahApps.Metro.Controls.Dialogs.BaseMetroDialog    Title$  تعديل الجهاز��5      $��RightToLeft=�5      8  2MahApps.Metro.Controls.Dialogs.DialogParticipation   Register 5
      �� .��5      �� -   5        $	��pnl��5      $��10q�6    � 	Resources 5   
    �� MergedDictionaries �� Source�� 5       5      �� 5      $ ..\..\Skins\MainSkin.xaml?�6)   %    5      .+�5   
   ��    TextBlockStyle#��[  6   $	��0,5q�6<   
النوع6P   �� 5   
   {�.   �� SelectedValuePath$ ID��6L    �� DisplayMemberPath$
 Name��6�    �� 
IsEditable .   ComboBoxStyle#��[ 5   �   ��5   '   �� *DevicesTypes+ �� SelectedItem 6c   �� *DeviceUpdate.DeviceType+	 �� SelectedValue	 5      �� *DeviceUpdate.DeviceTypeID+"
 �� ValidatesOnNotifyDataErrors
 .  �� UpdateSourceTrigger$ PropertyChanged=�5      �� 5   
   #��[  6   $	��0,5q�6<   اسم الجهاز6U   �� 5   
   -   5        $��First��5   K     TextBoxStyle#��[ 6�   !   Cafe.Helpers.FocusExtension   	IsFocused 6   �� *	IsFocused+��6Y   �� *DeviceUpdate.Name+
 . $ PropertyChanged=�.��6�   5      �� 5!   
   #��[  6   $	��0,5q�6<    الترتيب فى العرض6[   *  $MahApps.Metro.Controls.NumericUpDown  5"   
      HasDecimals .    NumericUpDownStyle2#��[ 6�      Value 66   �� *DeviceUpdate.Order+
 . $ PropertyChanged=�5#      )  #MahApps.Metro.Controls.ToggleSwitch  5%   
   $��Center=�6n      OnLabel$ متاح��6�      OffLabel$ غير متاح��6�     ToggleSwitchStyle#��[ 6�   {�6    �� *	NotBusy+   	IsChecked 6>   �� *DeviceUpdate.IsAvailable+6�   �� 5'   
   $��0 15 0 0q�6   �� 5(   
    �� Orientation$ 
Horizontal=�6   $��0 10 0 0q�6.   .+�5)      �� -   5        $	��btn��5)   &    �� 	IsDefault .   SquareButtonStyle#��[ 6L   $	��100��6w   ��61   �� *Update+.��5*      �� $ 
Horizontal=�6   .+�5+      �� $��
تحديث��6!   #��[  6/   $
��10,0q�6W   6g   .  (MahApps.Metro.IconPacks.PackIconMaterial  5,      �  �ControlzEx.PackIconBase`1[[MahApps.Metro.IconPacks.PackIconMaterialKind, MahApps.Metro.IconPacks, Version=1.9.1.0, Culture=neutral, PublicKeyToken=null]]   Kind$ Update=�61   5-      5.      �� 5/       �� IsCancel . $��Update��6E   #��[ 6_   $	��100��6�   $
��10,0q�6�   ��6%   �� *CloseDialog+.��50      �� $ 
Horizontal=�6   .+�51      �� $��
إغلاق��6!   #��[  6/   $
��10,0q�6W   6g   ,  &MahApps.Metro.IconPacks.PackIconModern  52      �	  �ControlzEx.PackIconBase`1[[MahApps.Metro.IconPacks.PackIconModernKind, MahApps.Metro.IconPacks, Version=1.9.1.0, Culture=neutral, PublicKeyToken=null]] 	  Kind$ Cancel=�6/   53      54      55      56      58      