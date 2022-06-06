# Common imports

import pandas as pd
import numpy as np
from sklearn import preprocessing
import seaborn as sns
sns.set(color_codes=True)
import matplotlib.pyplot as plt


merged_data = pd.read_csv('C:/Users/Amirashkan/Desktop/Unity/Shared Project/Backup/02142022/DT_WindFarm_MasterThesis/Assets/CSV/BearingTemp_3.csv')

# merged_data.index = pd.to_datetime(merged_data.index, format='%Y.%m.%d.%H.%M.%S',errors='ignore')
merged_data.head(800)
print(f'Total Data Points {merged_data.shape[0] + 1}')
# Visualising Data
ax = merged_data.plot(figsize = (16,6), title="Temperature Data" , legend = True)
ax.set(xlabel="Year-Month-Date", ylabel="Temperature (C)")
# plt.axvline(x='1/13/2010', linewidth=4, color='b', label ="Turbine Temp")
# plt.show()
# plt.text('2004-02-19 06:12:39',0.3,'Breakdown of Bearing 1',rotation=91, fontsize=14, color='b')


# Change to datetime format
Temp_data = merged_data.copy() 
# Temp_data[Temp_data.columns[0]] = pd.to_datetime(Temp_data[Temp_data.columns[0]], format='%Y-%m-%d')
Temp_data[Temp_data.columns[0]] = pd.to_datetime(Temp_data[Temp_data.columns[0]])
Temp_data = Temp_data.set_index(Temp_data.columns[0])
Temp_data = Temp_data.sort_index()
all_Turbines = Temp_data.astype('float')
# all_Turbines.tail()
# all_Turbines.head()
# merged_data["Date"] = pd.to_datetime(merged_data["Date"])

# fig = plt.figure()

# Divide the figure into a 1x2 grid, and give me the first section
# ax1 = fig.add_subplot(121)
# # Divide the figure into a 1x2 grid, and give me the second section
# ax2 = fig.add_subplot(122)

healthy = Temp_data['2011-09-01':'2014-12-10']
# healthy['Turbine3'].plot(figsize = (16,6), title="Healthy State" , legend = True, ax=ax1)
# ax1.set(xlabel="Month-Date Time", ylabel="Vibration/Acceleration(g)")

faulty = Temp_data['2014-12-10':'2015-07-02']
# ax2 = faulty['Turbine3'].plot(figsize = (16,6), title="Faulty State" , legend = True, ax= ax2)
# ax2.set(xlabel="Month-Date Time", ylabel="Vibration/Acceleration(g)")

healthy_bearing1 = Temp_data['2011-09-01':'2014-12-10']['Turbine3']

# Creating training dataframe
prophet_healthy_train = pd.DataFrame()
prophet_healthy_train['ds'] = healthy_bearing1.index
prophet_healthy_train['y'] = healthy_bearing1.values

# print(prophet_healthy_train.head())

healthy_bearing2 = Temp_data['2014-12-10':'2015-07-02']['Turbine3']

# Creating training dataframe
prophet_healthy_train2 = pd.DataFrame()
prophet_healthy_train2['ds'] = healthy_bearing2.index
prophet_healthy_train2['y'] = healthy_bearing2.values

# prophet_healthy_train2.head()

from fbprophet import Prophet
m = Prophet(interval_width = 1)
# Using the training data from "healthy part"
m.fit(prophet_healthy_train)
# forecast = m.fit(prophet_healthy_train)
# forecast[['ds', 'yhat', 'yhat_lower', 'yhat_upper']].tail()


forecast = m.predict(prophet_healthy_train)
forecast['fact'] = prophet_healthy_train['y'].reset_index(drop = True)
print('Displaying Prophet plot')
fig1 = m.plot(forecast,uncertainty = True,plot_cap = True)
fig1 = healthy_bearing1.plot(figsize = (16,6), title="Fit of Training Data", legend="Evaluation")
fig1.set(xlabel="Month (MM)-Date(DD) Time", ylabel="Temperature(C)")
fig1.legend(["True data","Trend line",'Expected values','Acceptable variance'])
# fig1 = healthy_bearing2.plot(figsize = (16,6), title="Fit of Training Data", legend="total")
#The black points are the true data points of the Temperature sensor. 
#The blue line represents the fitted line (trend line) with the light blue portion showing the acceptable variance.

# fig1 = m.plot(forecast)
# fig1 = healthy_bearing1.plot(figsize = (16,6), title="Fit of Training Data")
# fig1.set(xlabel="Month (MM)-Date(DD) Time", ylabel="Temperature(C)")
# # fig1 = healthy_bearing2.plot(figsize = (16,6), title="Fit of Training Data")
# fig1.legend(["True data","Trend line",'Expected values','Real data','Acceptable variance'])

prophet_faultydata = Temp_data['2014-12-10':'2015-08-02']['Turbine3']
prophet_faultydata.head()

prophet_faulty_test = pd.DataFrame()

prophet_faulty_test['ds'] = prophet_faultydata.index
#pd.to_datetime(prophet_healthy.index, format='%Y.%m.%d.%H.%M.%S')
prophet_faulty_test['y'] = prophet_faultydata.values

forecast = m.predict(prophet_faulty_test)
forecast['fact'] = prophet_faulty_test['y'].reset_index(drop = True)
print('Displaying Prophet plot')
fig1 = m.plot(forecast)
fig1 = prophet_faultydata.plot(figsize = (16,6),title="Fit of Test/Unseen/Fault Data",linewidth = 1.0, linestyle='-',color='red')
fig1.set(xlabel="Month (MM)-Date(DD) Time", ylabel="Temperature(C))")
fig1.legend(["Actual/Healthy Data","Expected/Predicted",'Actual/Faulty Data','Acceptable variance'])
plt.plot()
plt.show()
plt.ion()
# fig1.text(7,3,'Expected/Predicted', fontsize=14, color='r')
# fig1.text(7,3,'Actual/Faulty Data', fontsize=14, color='r')
# fig1.text(7,3,'Actual/Healthy', fontsize=14, color='r')


#The black points are the true data points of the temperature sensor. 
#The blue line represents the expected values from 2004-02-15, 23:42:39 with the light blue portion showing 
#the acceptable variance.

#Clearly, the values are higher than the predicted values and an alarm can be sounded.